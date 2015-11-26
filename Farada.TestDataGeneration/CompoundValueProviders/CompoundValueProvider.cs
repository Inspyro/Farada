﻿using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Modifiers;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// The compound value provider is a <see cref="ITestDataGenerator"/> that can fill compound types such as classes
  /// based on a domain declaration, where various MemberChains, TypeChains and AttributeChains are declared
  /// </summary>
  internal class CompoundValueProvider : ITestDataGenerator
  {
    private readonly HashSet<IKey> _autoFillMapping;
    private readonly InstanceFactory _instanceFactory;
    private readonly ModificationFactory _modificationFactory;

    public Random Random { get; }

    internal CompoundValueProvider (ValueProviderDictionary valueProviderDictionary, HashSet<IKey> autoFillMapping, Random random, IList<IInstanceModifier> instanceModifiers, IParameterConversionService parameterConversionService)
    {
      Random = random;
      _autoFillMapping = autoFillMapping;
      _instanceFactory = new InstanceFactory (this, valueProviderDictionary, parameterConversionService);
      _modificationFactory = new ModificationFactory (instanceModifiers, random);
    }

    public TValue Create<TValue> (int maxRecursionDepth = 2, IFastMemberWithValues member = null)
    {
      return CreateMany<TValue> (1, maxRecursionDepth, member).Single();
    }

    public IReadOnlyList<TValue> CreateMany<TValue> (
        int numberOfObjects,
        int maxRecursionDepth = 2,
        IFastMemberWithValues member = null)
    {
      var rootKey = member == null
          ? (IKey) new TypeKey (typeof (TValue))
          : new ChainedKey (typeof (TValue), member);

      var instances = CreateMany (rootKey, numberOfObjects, maxRecursionDepth);

      return instances.CastOrDefault<TValue>().ToList();
    }

    ///Note: The create many method is optimized in many ways
    ///1: If you create 100 Dog objects it will first created 100 instances of Dog (with a fast version of the Activator)
    ///   Then it searches the value provider for each member and generates 100 values per member for each instance of Dog
    /// 2: It uses a cached but thread safe version of reflection <see cref="FastReflectionUtility"/>
    public IList<object> CreateMany (IKey currentKey, int numberOfObjects, int maxRecursionDepth)
    {
      //we check for recursion depth in order to avoid endless recursions (e.g. a->b->a->b->a->...)
      if (currentKey.RecursionDepth >= maxRecursionDepth)
        throw new ArgumentException ("The current key " + currentKey + " had an higher recursion depth than maxRecursionDepth");

      //Here we create the actual instances of the type (these are values created by either the fast activator or the registered value providers)
      var instances = _instanceFactory.CreateInstances (currentKey, numberOfObjects);

      //now that we have valid instances we can modify them all in a batch by the registered instance modifiers (example: a null modifier, that makes 30% of the instances null)
      instances = _modificationFactory.ModifyInstances (currentKey, instances);

      //non-auto filled instances can be returned instantly. Others are filled below.
      var startKey = currentKey;
      while (startKey != null)
      {
        if (_autoFillMapping.Contains (startKey))
          return instances;

        //we also have to check the previous (more generic) keys.
        startKey = startKey.PreviousKey;
      }

      //check if type injection has taken place and split the base type into the correct sub types (important for base class properties)
      var typeToInstances = SubTypeInstanceHolder.SplitUpSubTypes (currentKey, instances);

      //now we iterate over all instances for each sub type (this is more efficient)
      foreach (var instancesForType in typeToInstances.Values)
      {
        //if the sub type cannot be instantiated, we just skip it, it will not be filled (maybe it is a value type)
        if (!instancesForType.Key.Type.IsCompoundType())
          continue;

        //now we reflect the properties of the concrete sub type (note:this is cached in a concurrent dictionary) 
        var members = FastReflectionUtility.GetTypeInfo (instancesForType.Key.Type).Members;

        //now we fill each member
        foreach (var member in members)
        {
          //first we need to create the key that the member has in our creator chain
          var nextKey = instancesForType.Key.CreateKey (member);

          //in case we reached the max recursion depth for this member we skip filling it.
          if (nextKey.RecursionDepth >= maxRecursionDepth)
            continue;

          //next we will recursively call this function (in case the member is compound/complex)
          //Note: we create many values, in order to execute the logic of the method less often
          try
          {
            var memberValues = CreateMany (nextKey, instancesForType.Instances.Count, maxRecursionDepth);
            //now we iterate over all created values and set them for the corresponding previously created instance
            //Example: if you call CreatMany<Dog>(100) - all dog properties will be created 100 times and filled into the 100 dog instances
            for (var i = 0; i < instancesForType.Instances.Count; ++i)
            {
              //Of course some of the instances might be null (due to modifieres or sub types or sub type value providers or randomness of value providers)
              //we can safely skip them
              if (instancesForType.Instances[i] == null)
                continue;

              //in case we got a value we set the member
              member.SetValue (instancesForType.Instances[i], memberValues[i]);
            }
          }
          catch (Exception ex)
          {
            throw new NotSupportedException (
                "Could not auto-fill " + currentKey + " (member " + member.Name + "). Please provide a value provider",
                ex);
          }
        }
      }

      //here we return the instances that were created and filled (with all properties and sub properties)
      return instances;
    }
  }
}