using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Modifiers;
using Farada.TestDataGeneration.ValueProviders;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// The compound value provider is a <see cref="ITestDataGenerator"/> that can fill compound types such as classes
  /// based on a domain declaration, where various MemberChains, TypeChains and AttributeChains are declared
  /// </summary>
  internal class CompoundValueProvider : ITestDataGenerator, ITestDataGeneratorAdvanced
  {
    private readonly ValueProviderDictionary _valueProviderDictionary;
    private readonly HashSet<IKey> _autoFillMapping;
    private readonly IMemberSorter _memberSorter;
    private readonly IMetadataResolver _metadataResolver;
    private readonly IFastReflectionUtility _fastReflectionUtility;
    private readonly InstanceFactory _instanceFactory;
    private readonly ModificationFactory _modificationFactory;

    

    public IRandom Random { get; }

    internal CompoundValueProvider (
        ValueProviderDictionary valueProviderDictionary,
        HashSet<IKey> autoFillMapping,
        IMemberSorter memberSorter,
        IMetadataResolver metadataResolver,
        IRandom random,
        IList<IInstanceModifier> instanceModifiers,
        IParameterConversionService parameterConversionService,
        IFastReflectionUtility fastReflectionUtility)
    {
      Random = random;
      _valueProviderDictionary = valueProviderDictionary;
      _autoFillMapping = autoFillMapping;
      _memberSorter = memberSorter;
      _metadataResolver = metadataResolver;
      _fastReflectionUtility = fastReflectionUtility;
      _instanceFactory = new InstanceFactory (
          this,
          valueProviderDictionary,
          _memberSorter,
          _metadataResolver,
          parameterConversionService,
          fastReflectionUtility);
      _modificationFactory = new ModificationFactory (instanceModifiers, random);
    }

    public TValue Create<TValue>(int maxRecursionDepth = 2, IFastMemberWithValues member = null)
    {
      return (TValue)Create(typeof(TValue), maxRecursionDepth, member);
    }

    public IList<TValue> CreateMany<TValue>(
        int numberOfObjects,
        int maxRecursionDepth = 2,
        IFastMemberWithValues member = null)
    {
      return CreateMany (typeof (TValue), numberOfObjects, maxRecursionDepth, member).CastOrDefault<TValue>().ToList();
    }

    public object Create(Type type, int maxRecursionDepth = 2, IFastMemberWithValues member = null)
    {
      return CreateMany(type, 1, maxRecursionDepth, member).Single();
    }

    public IList<object> CreateMany(Type type, int numberOfObjects, int maxRecursionDepth = 2, IFastMemberWithValues member = null)
    {
      var rootKey = member == null
          ? (IKey)new TypeKey(type)
          : new ChainedKey(type, member);

      return CreateMany (rootKey, null, numberOfObjects, maxRecursionDepth);
    }

    ///Note: The create many method is optimized in many ways
    ///1: If you create 100 Dog objects it will first created 100 instances of Dog (with a fast version of the Activator)
    ///   Then it searches the value provider for each member and generates 100 values per member for each instance of Dog
    /// 2: It uses a cached but thread safe version of reflection <see cref="FastReflectionUtility"/>
    public IList<object> CreateMany (
        IKey currentKey,
        [CanBeNull] IList<object> resolvedMetadatasForKey,
        int itemCount,
        int maxRecursionDepth)
    {
      //we check for recursion depth in order to avoid endless recursions (e.g. a->b->a->b->a->...)
      if (currentKey.RecursionDepth >= maxRecursionDepth)
        throw new ArgumentException ("The current key " + currentKey + " had an higher recursion depth than maxRecursionDepth");

      //Here we create the actual instances of the type (these are values created by either the fast activator or the registered value providers)
      var instances = _instanceFactory.CreateInstances (currentKey, resolvedMetadatasForKey, itemCount);

      //now that we have valid instances we can modify them all in a batch by the registered instance modifiers (example: a null modifier, that makes 30% of the instances null)
      instances = _modificationFactory.ModifyInstances (currentKey, instances);

      //non-auto filled instances can be returned instantly. Others are filled below.
      if (!ShouldBeFilled(currentKey))
        return instances;

      //check if type injection has taken place and split the base type into the correct sub types (important for base class properties)
      var typeToInstances = SubTypeInstanceHolder.SplitUpSubTypes (currentKey, instances);

      //now we iterate over all instances for each sub type (this is more efficient)
      foreach (var instancesForType in typeToInstances.Values)
      {
        //if the sub type cannot be instantiated, we just skip it, it will not be filled (maybe it is a value type)
        if (!instancesForType.Key.Type.IsCompoundType())
          continue;

        //now we reflect the properties of the concrete sub type (note:this is cached in a concurrent dictionary) 
        var members = _fastReflectionUtility.GetTypeInfo (instancesForType.Key.Type).Members;

        //now we sort the members by dependency.
        var sortedMembers = _memberSorter.Sort (members, instancesForType.Key).ToList();

        //now we fill each member
        for (int index = 0; index < sortedMembers.Count; index++)
        {
          var member = sortedMembers[index];
          //first we need to create the key that the member has in our creator chain
          var memberKey = instancesForType.Key.CreateKey (member);

          //in case we reached the max recursion depth for this member we skip filling it.
          if (memberKey.RecursionDepth >= maxRecursionDepth)
            continue;

          List<object> resolvedMetadatas = null;
          if (_metadataResolver.NeedsMetadata (memberKey))
          {
            //we map and resolve/fill the metadatas (from the instances):
            //the values of the dependencies can only be filled correctly, due to the fact that we sorted them beforehand.
            var metadataContexts =
                GetMetadataContexts (instancesForType.Key, sortedMembers.Take (index), instancesForType.Instances).ToList();

            resolvedMetadatas = _metadataResolver.Resolve (memberKey, metadataContexts)?.ToList();
          }

          //next we will recursively call this function (in case the member is compound/complex)
          //Note: we create many values, in order to execute the logic of the method less often
          try
          {
            var memberValues = CreateMany (memberKey, resolvedMetadatas, itemCount, maxRecursionDepth);
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

    private IEnumerable<MetadataObjectContext> GetMetadataContexts (
        IKey baseKey,
        IEnumerable<IFastMemberWithValues> dependendMembers,
        IList<object> instances)
    {
      var instanceToContext = new Dictionary<object, MetadataObjectContext>();
      foreach (var member in dependendMembers)
      {
        var memberKey = baseKey.CreateKey (member);
        foreach (var instance in instances)
        {
          if (!instanceToContext.ContainsKey (instance))
            instanceToContext[instance] = new MetadataObjectContext();

          instanceToContext[instance].Add (memberKey, member.GetValue (instance));
        }
      }

      return instanceToContext.Select (i => i.Value);
    }

    private bool ShouldBeFilled (IKey startKey)
    {
      if (_autoFillMapping.Contains (startKey))
        return true; //if auto fill is "enforced" we always fill the current key (explicitly set by user). 

      return !_valueProviderDictionary.IsTypeFilledByValueProvider(startKey); //when the type is not filled, we fill it.
    }
  }
}