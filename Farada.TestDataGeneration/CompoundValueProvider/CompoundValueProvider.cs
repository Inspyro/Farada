using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProvider.Keys;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Modifiers;
using Farada.TestDataGeneration.ValueProvider;

namespace Farada.TestDataGeneration.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  internal class CompoundValueProvider : ICompoundValueProvider
  {
    private readonly ValueProviderDictionary _valueProviderDictionary;
    private readonly IList<IInstanceModifier> _instanceModifiers;

    private readonly Random _random;

    internal CompoundValueProvider (ValueProviderDictionary valueProviderDictionary, Random random, IList<IInstanceModifier> instanceModifiers)
    {
      _valueProviderDictionary = valueProviderDictionary;
      _instanceModifiers = instanceModifiers;
      _random = random;
    }

    public TValue Create<TValue> (int maxRecursionDepth = 2, FastReflection.IFastPropertyInfo propertyInfo=null)
    {
      return CreateMany<TValue>(1, maxRecursionDepth, propertyInfo).Single();
    }

    public IReadOnlyList<TValue> CreateMany<TValue> (int numberOfObjects, int maxRecursionDepth = 2, FastReflection.IFastPropertyInfo propertyInfo=null)
    {
      var rootKey = propertyInfo == null
          ? (IKey) new TypeKey(typeof (TValue))
          : new ChainedKey(typeof (TValue), propertyInfo);

      var instances = CreateMany(rootKey, numberOfObjects, maxRecursionDepth);

      return instances == null ? CreateManyDefaultObjects<TValue>(numberOfObjects) : instances.CastOrDefault<TValue>().ToList();
    }

    private static IReadOnlyList<TValue> CreateManyDefaultObjects<TValue> (int numberOfObjects)
    {
      return EnumerableExtensions.Repeat(() => default(TValue), numberOfObjects).ToList();
    }

    ///Note: The create many method is optimized in many ways
    ///1: If you create 100 Dog objects it will first created 100 instances of Dog (with a fast version of the Activator)
    ///   Then it will create 100 properties of each type, by calling the value provider it search only once per property 100 times
    ///   Then it will fill those 100 properties
    /// 2: It uses a cached but thread safe version of reflection call <see cref="FastReflection"/>
    private IList<object> CreateMany (IKey currentKey, int numberOfObjects, int maxRecursionDepth)
    {
      //we check for recursion depth in order to avoid endless recursions (e.g. a->b->a->b->a->...)
      if (currentKey.RecursionDepth >= maxRecursionDepth)
        return null;

      //Here we create the actual instances of the type (this are values created by either the fast activator or the registered value providers)
      var instances = CreateInstances(currentKey, numberOfObjects);
      
      //in case the instances were not created (no value provider, not instantiable, we just return null values, that will later be casted to default(T))
      if (instances == null)
        return null;
      
      //now that we have valid instances we can modify them all in a batch by the registered modifiers (example: a null modifier, that makes 30% of the instances null)
      instances = ModifyInstances(currentKey, instances);

      //check if type injection has taken place and split the base type into the correct sub types (important for base class properties)
      var typeToInstances = SplitUpSubTypes(currentKey, instances);

      //no we go over all instances for each sub type (this is more effient)
      foreach (var instancesForType in typeToInstances.Values)
      {
        //if the sub type cannot be instantiated, we just skip it, it will not be filled (maybe it is a value type)
        if (!instancesForType.Key.PropertyType.IsCompoundType())
          continue;

        //now we reflect the properties of the concrete sub type (note:this is cached in a concurrent dictionary) 
        var properties = FastReflection.FastReflection.GetTypeInfo(instancesForType.Key.PropertyType).Properties;

        //now we fill each property
        foreach (var property in properties)
        {
          //first we need to create the key that the property has in our creator chain
          var nextKey = instancesForType.Key.CreateKey(property);

          //next we will recursivly call this function (in case the property is compound/complex)
          //Note: we create many values, in order to execute the logic of the method less often
          var propertyValues = CreateMany(nextKey, instancesForType.Instances.Count, maxRecursionDepth);

          //in case we could not fill the property we just skip filling it, leave it to it's current (probably default) value (e.g. because it was not suported) 
          if (propertyValues == null)
            continue;

          //no we go over all created values and set them for the corresponding previously created instane
          //Example: if you call CreatMany<Dog>(100) - all dog properties will be created 100 times and filled into the 100 dog instances
          for (var i = 0; i < instancesForType.Instances.Count; ++i)
          {
            //Of course some of the instances might be null (due to modifieres or sub types or sub type value providers or randomness of value providers)
            //we can safely skip them
            if (instancesForType.Instances[i] == null)
              continue;

            //in case we got a value we set the property
            property.SetValue(instancesForType.Instances[i], propertyValues[i]);
          }
        }
      }

      //here we return the instances that were created and filled (with all properties and sub properties)
      return instances;
    }

    private static Dictionary<Type, SpecialInstanceHolder> SplitUpSubTypes (IKey currentKey, IList<object> instances)
    {
      if (instances.Where(instance=>instance!=null).All(instance => instance.GetType() == currentKey.PropertyType))
      {
        return new Dictionary<Type, SpecialInstanceHolder> { { currentKey.PropertyType, new SpecialInstanceHolder(currentKey, instances) } };
      }

      var typeToInstances = new Dictionary<Type, SpecialInstanceHolder>();
      foreach (var instance in instances.Where(instance => instance != null))
      {
        var concreteType = instance.GetType();
        if (!typeToInstances.ContainsKey(concreteType))
        {
          typeToInstances.Add(
              concreteType,
              new SpecialInstanceHolder(currentKey.ChangePropertyType(concreteType)));
        }

        typeToInstances[concreteType].Instances.Add(instance);
      }

      return typeToInstances;
    }

    private IList<object> ModifyInstances (IKey currentKey, IList<object> instances)
    {
      return _instanceModifiers.Aggregate(
          instances,
          (current, instanceModifier) =>
              instanceModifier.Modify(new ModificationContext(currentKey.PropertyType, currentKey.Property, _random), current));
    }

    private IList<object> CreateInstances (IKey key, int numberOfObjects)
    {
      var rootLink = _valueProviderDictionary.GetLink(key);
      return CreateInstances(key, rootLink == null ? null : rootLink.Value, CreateValueProviderContext(rootLink, key), numberOfObjects);
    }

    private static IList<object> CreateInstances (IKey key, IValueProvider valueProvider, IValueProviderContext valueProviderContext, int numberOfObjects)
    {
      if (valueProvider == null || valueProviderContext == null)
      {
        if (!key.PropertyType.CanBeInstantiated())
          return null;

        var propertyFactory = FastActivator.GetFactory(key.PropertyType);
        return EnumerableExtensions.Repeat(() => propertyFactory(), numberOfObjects).ToList();
      }

      return EnumerableExtensions.Repeat(() => valueProvider.CreateValue(valueProviderContext), numberOfObjects).ToList();
    }

    private IValueProviderContext CreateValueProviderContext (ValueProviderLink providerLink, IKey key)
    {
      if (providerLink == null)
        return null;

      var previousLink = providerLink.Previous==null?null:providerLink.Previous();
      var previousContext = previousLink == null ? null : CreateValueProviderContext(previousLink, key);

      return providerLink.Value.CreateContext(
          new ValueProviderObjectContext(
              this,
              _random,
              () => previousLink == null ? null : CreateInstances(previousLink.Key, previousLink.Value, previousContext, 1).Single(),
              key.PropertyType,
              key.Property));
    }

  }

  internal class SpecialInstanceHolder
  {
    public IKey Key { get; private set; }
    public IList<object> Instances { get; private set; }  

    public SpecialInstanceHolder (IKey currentKey, IList<object> instances=null)
    {
      Key = currentKey;
      Instances = instances??new List<object>();
    }
  }
}