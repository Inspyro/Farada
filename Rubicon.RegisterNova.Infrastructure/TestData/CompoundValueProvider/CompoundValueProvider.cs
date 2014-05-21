using System;
using System.Collections.Generic;
using System.Linq;
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

    internal CompoundValueProvider (ValueProviderDictionary valueProviderDictionary, Random random, IList<IInstanceModifier> instanceModifiers)
    {
      _valueProviderDictionary = valueProviderDictionary;
      _instanceModifiers = instanceModifiers;
      Random = random;
    }

    // TODO: Unschönheit, warum muss das public sein?
    public Random Random { get; private set; }

    public TValue Create<TValue> (int maxRecursionDepth = 2, IFastPropertyInfo propertyInfo=null)
    {
      return CreateMany<TValue>(1, maxRecursionDepth, propertyInfo).Single();
    }

    public ICollection<TValue> CreateMany<TValue> (int numberOfObjects, int maxRecursionDepth = 2, IFastPropertyInfo propertyInfo=null)
    {
      var rootKey = new Key(typeof (TValue), propertyInfo);
      var instances = CreateMany(rootKey, numberOfObjects, maxRecursionDepth);

      return instances == null ? CreateManyDefaultObjects<TValue>(numberOfObjects) : instances.CastOrDefault<TValue>().ToList();
    }

    private static ICollection<TValue> CreateManyDefaultObjects<TValue> (int numberOfObjects)
    {
      return EnumerableExtensions.Repeat(() => default(TValue), numberOfObjects).ToList();
    }

    // TODO: Maybe add some comments to algorithm?
    private IList<object> CreateMany (Key currentKey, int numberOfObjects, int maxRecursionDepth)
    {
      var instances = CreateInstances(currentKey, numberOfObjects);

      if (instances != null)
        instances = ModifyInstances(currentKey, instances);

      if (instances == null || !currentKey.Top.PropertyType.IsCompoundType() || currentKey.RecursionDepth > maxRecursionDepth)
        return instances;

      var properties = FastReflection.FastReflection.GetTypeInfo(currentKey.Top.PropertyType).Properties;
      foreach (var property in properties)
      {
        var nextKey = currentKey.GetNextKey(property);

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

    private IList<object> ModifyInstances (Key currentKey, IList<object> instances)
    {
      return _instanceModifiers.Aggregate(
          instances,
          (current, instanceModifier) =>
              instanceModifier.Modify(new ModificationContext(currentKey.Top.PropertyType, currentKey.Top.Property, Random), current));
    }

    private IList<object> CreateInstances(Key key, int numberOfObjects)
    {
      var attributeProviderLink = key.Top.Property == null
          ? null
          : key.Top.Property.Attributes.Select(a => _valueProviderDictionary.GetLink(new Key(a))).SingleOrDefault(link => link != null);

     var rootLink = _valueProviderDictionary.GetLink(key);

      if(attributeProviderLink!=null)
      {
        attributeProviderLink = new ValueProviderLink(
            attributeProviderLink.Value,
            attributeProviderLink.Key,
            () => _valueProviderDictionary.GetLink(key));

        rootLink = attributeProviderLink;
      }

      return CreateInstances(key, rootLink==null?null:rootLink.Value, CreateValueProviderContext(rootLink, key), numberOfObjects);
    }

    private static IList<object> CreateInstances (Key key, IValueProvider valueProvider, IValueProviderContext valueProviderContext, int numberOfObjects)
    {
      if (valueProvider == null||valueProviderContext==null)
      {
        return key.Top.PropertyType.CanBeInstantiated()
            ? EnumerableExtensions.Repeat(() => Activator.CreateInstance(key.Top.PropertyType), numberOfObjects).ToList()
            : null;
      }

      return EnumerableExtensions.Repeat(() => valueProvider.CreateObjectValue(valueProviderContext), numberOfObjects).ToList();
    }

    private IValueProviderContext CreateValueProviderContext (ValueProviderLink providerLink, Key key)
    {
      if (providerLink == null)
        return null;

      var previousLink = providerLink.Previous==null?null:providerLink.Previous();
      var previousContext = previousLink == null ? null : CreateValueProviderContext(previousLink, key);

      return providerLink.Value.CreateContext(
          this,
          Random,
          () => previousLink == null ? null : CreateInstances(previousLink.Key, previousLink.Value, previousContext, 1).Single(),
          key.Top.PropertyType,
          key.Top.Property);
    }

  }
}