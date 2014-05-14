using System;
using System.Collections.Generic;
using System.Linq;
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

    internal CompoundValueProvider (ValueProviderDictionary valueProviderDictionary, Random random)
    {
      _valueProviderDictionary = valueProviderDictionary;
      Random = random;
    }

    // TODO: Unschönheit, warum muss das public sein?
    public Random Random { get; private set; }

    public TValue Create<TValue> (int maxRecursionDepth = 2)
    {
      return CreateMany<TValue>(1, maxRecursionDepth).Single();
    }

    public ICollection<TValue> CreateMany<TValue> (int numberOfObjects, int maxRecursionDepth = 2)
    {
      var rootKey = new Key(typeof (TValue));
      var instances = CreateMany(rootKey, numberOfObjects, maxRecursionDepth);

      return instances == null ? CreateManyDefaultObjects<TValue>(numberOfObjects) : instances.Cast<TValue>().ToList();
    }

    private static ICollection<TValue> CreateManyDefaultObjects<TValue> (int numberOfObjects)
    {
      return EnumerableExtensions.Repeat(() => default(TValue), numberOfObjects).ToList();
    }

    // TODO: Maybe add some comments to algorithm?
    private IList<object> CreateMany (Key currentKey, int numberOfObjects, int maxRecursionDepth)
    {
      IList<object> instances = null;

      var valueProvider = _valueProviderDictionary.GetValueProviderFor(currentKey);
      if (valueProvider != null)
      {
        instances = CreateInstances(currentKey, numberOfObjects);
      }
      else if (currentKey.Top.PropertyType.CanBeInstantiated())
      {
        instances = EnumerableExtensions.Repeat(() => Activator.CreateInstance(currentKey.Top.PropertyType), numberOfObjects).ToList();
      }

      if (instances == null || !currentKey.Top.PropertyType.IsCompoundType() || ReachedMaxRecursionDepth(currentKey, maxRecursionDepth))
        return instances;
      
      var properties = FastReflection.FastReflection.GetTypeInfo(currentKey.Top.PropertyType).Properties;
      foreach (var property in properties)
      {
        var nextKey = currentKey.GetNextKey(property.PropertyType, property);

        var propertyValues = CreateMany(nextKey, numberOfObjects, maxRecursionDepth);
        if (propertyValues == null)
          continue;

        for (var i = 0; i < numberOfObjects; ++i)
          property.SetValue(instances[i], propertyValues[i]);
      }

      return instances;
    }

    private IList<object> CreateInstances (Key key, int numberOfObjects)
    {
      if (key == null)
        return null;

      var previousValueProvider = _valueProviderDictionary.GetValueProviderFor(key);
      if (previousValueProvider == null)
        return null;

      var previousContext = CreateValueProviderContext(key);
      return EnumerableExtensions.Repeat(() => previousValueProvider.CreateObjectValue(previousContext), numberOfObjects).ToList();
    }

    private ValueProviderContext CreateValueProviderContext (Key key)
    {
      var previousKey = key.GetPreviousKey();
      return new ValueProviderContext(this, Random, key.Top.Property, () => CreateInstances(previousKey, 1).Single());
    }

    private static bool ReachedMaxRecursionDepth (Key currentKey, int maxDepth)
    {
      return currentKey.RecursionDepth > maxDepth;
    }
  }
}