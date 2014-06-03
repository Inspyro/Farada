using System;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider.Keys;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
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

    public TValue Create<TValue> (int maxRecursionDepth = 2, IFastPropertyInfo propertyInfo=null)
    {
      return CreateMany<TValue>(1, maxRecursionDepth, propertyInfo).Single();
    }

    public IReadOnlyList<TValue> CreateMany<TValue> (int numberOfObjects, int maxRecursionDepth = 2, IFastPropertyInfo propertyInfo=null)
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

    // TODO: Maybe add some comments to algorithm?
    private IList<object> CreateMany (IKey currentKey, int numberOfObjects, int maxRecursionDepth)
    {
      if (currentKey.RecursionDepth >= maxRecursionDepth)
        return null;

      var instances = CreateInstances(currentKey, numberOfObjects);

      if (instances != null)
        instances = ModifyInstances(currentKey, instances);

      if (instances == null)
        return null;

      //check if type injection has taken place (important for base class properties)
      var typeToInstances = SplitUpSubTypes(currentKey, instances);
      foreach (var instancesForType in typeToInstances.Values)
      {
        if (!instancesForType.Key.PropertyType.IsCompoundType())
          continue;

        var properties = FastReflection.FastReflection.GetTypeInfo(instancesForType.Key.PropertyType).Properties;
        foreach (var property in properties)
        {
          var nextKey = instancesForType.Key.CreateKey(property);

          var propertyValues = CreateMany(nextKey, instancesForType.Instances.Count, maxRecursionDepth);
          if (propertyValues == null)
            continue;

          for (var i = 0; i < instancesForType.Instances.Count; ++i)
          {
            if (instancesForType.Instances[i] == null)
              continue;

            property.SetValue(instancesForType.Instances[i], propertyValues[i]);
          }
        }
      }

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