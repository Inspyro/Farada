using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
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
      var instances = CreateInstances(currentKey, numberOfObjects);

      if (instances == null || !currentKey.Top.PropertyType.IsCompoundType() || ReachedMaxRecursionDepth(currentKey, maxRecursionDepth))
        return instances;
      
      var properties = FastReflection.FastReflection.GetTypeInfo(currentKey.Top.PropertyType).Properties;
      foreach (var property in properties)
      {
        var nextKey = currentKey.GetNextKey(property);

        var propertyValues = CreateMany(nextKey, numberOfObjects, maxRecursionDepth);
        if (propertyValues == null)
          continue;

        for (var i = 0; i < numberOfObjects; ++i)
          property.SetValue(instances[i], propertyValues[i]);
      }

      return instances;
    }

    private IList<object> CreateInstances (Key key, int numberOfObjects, IValueProvider valueProviderToExclude=null)
    {
      if (key == null)
        return null;

      var valueProvider = _valueProviderDictionary.GetValueProviderFor(key, valueProviderToExclude);
      if (valueProvider == null)
      {
        return key.Top.PropertyType.CanBeInstantiated()
            ? EnumerableExtensions.Repeat(() => Activator.CreateInstance(key.Top.PropertyType), numberOfObjects).ToList()
            : null;
      }

      var previousContext = CreateValueProviderContext(key, valueProvider);
      return EnumerableExtensions.Repeat(() => valueProvider.CreateObjectValue(previousContext), numberOfObjects).ToList();
    }

    private IValueProviderContext CreateValueProviderContext (Key key, IValueProvider valueProvider)
    {
      var previousKey = key.GetPreviousKey();
      return valueProvider.CreateContext(this, Random, () => CreateInstances(previousKey, 1, valueProvider).Single(), key.Top.Property);
    }

    private static bool ReachedMaxRecursionDepth (Key currentKey, int maxDepth)
    {
      return currentKey.RecursionDepth > maxDepth;
    }
  }
}