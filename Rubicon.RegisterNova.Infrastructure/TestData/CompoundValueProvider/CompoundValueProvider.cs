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
      var instances = CreateInstances(currentKey, numberOfObjects);

      if (instances != null)
        instances = ModifyInstances(currentKey, instances);

      if (instances == null || !currentKey.PropertyType.IsCompoundType() || currentKey.RecursionDepth >= maxRecursionDepth)
        return instances;

      var properties = FastReflection.FastReflection.GetTypeInfo(currentKey.PropertyType).Properties;
      foreach (var property in properties)
      {
        var nextKey = currentKey.CreateKey(property);

        var propertyValues = CreateMany(nextKey, numberOfObjects, maxRecursionDepth);
        if (propertyValues == null)
          continue;

        for (var i = 0; i < numberOfObjects; ++i)
        {
          if (instances[i] == null)
            continue;

          property.SetValue(instances[i], propertyValues[i]);
        }
      }

      return instances;
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
      if (valueProvider == null||valueProviderContext==null)
      {
        return key.PropertyType.CanBeInstantiated()
            ? EnumerableExtensions.Repeat(() => Activator.CreateInstance(key.PropertyType), numberOfObjects).ToList()
            : null;
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
}