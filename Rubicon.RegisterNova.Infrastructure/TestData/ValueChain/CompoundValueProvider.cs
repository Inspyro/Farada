using System;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.Reflection;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class CompoundValueProvider
  {
    private readonly ValueProviderDictionary _dictionary;
    private Dictionary<Type, int> _typeFillCountDictionary;

    internal CompoundValueProvider (ValueProviderDictionary valueProviderDictionary, Random random)
    {
      _dictionary = valueProviderDictionary;
      Random = random;
    }

    public Random Random { get; private set; }

    public TValue Create<TValue> (int maxDepth = 2)
    {
      return CreateMany<TValue>(1, maxDepth).Single();
    }

    public IEnumerable<TValue> CreateMany<TValue> (int numberOfObjects, int maxDepth = 2)
    {
      _typeFillCountDictionary = new Dictionary<Type, int>();

      var key = new Key(new KeyPart(typeof (TValue)));
      var values = Create(numberOfObjects, key, maxDepth);
      if (values != null)
        return values.Cast<TValue>();

      return EnumerableExtensions.Repeat(() => default(TValue), numberOfObjects);
    }

    private IList<object> Create (int numberOfObjects, Key currentKey, int maxDepth)
    {
      IList<object> instances = null;

      var valueProvider = _dictionary.GetValueProviderFor(currentKey);
      if(valueProvider!=null)
      {
        instances = GetValue(numberOfObjects, currentKey);
      }
      else if (currentKey.GetTopType().CanBeInstantiated())
      {
        
        instances = EnumerableExtensions.Repeat(()=>Activator.CreateInstance(currentKey.GetTopType()), numberOfObjects).ToList();
      }

      if (instances == null || !MayFill(currentKey.GetTopType(), maxDepth))
        return instances;

      RaiseFillCount(currentKey.GetTopType());

      if (currentKey.GetTopType().IsCompoundType())
      {
        //we go over all properties and generate values for them
        var properties = FastReflection.GetTypeInfo(currentKey.GetTopType()).Properties;
        foreach (var property in properties)
        {
          var nextKey = currentKey.GetNextKey(property.PropertyType, property.Name);

          var propertyValues = Create(numberOfObjects, nextKey, maxDepth);
          if (propertyValues != null)
          {
            for(int i = 0; i < numberOfObjects; ++i)
              property.SetValue(instances[i], propertyValues[i]);
          }
        }
      }

      return instances;
    }

    private IList<object> GetValue (int numberOfObjects, Key key)
    {
      if (key == null)
        return null;

      var previousValueProvider = _dictionary.GetValueProviderFor(key);
      if (previousValueProvider == null)
        return null;

      var previousContext = CreateValueProviderContext(key);
      return EnumerableExtensions.Repeat(() => previousValueProvider.GetObjectValue(previousContext), numberOfObjects).ToList();
    }

    private ValueProviderContext CreateValueProviderContext (Key key)
    {
      var previousKey = key.GetPreviousKey();
      return new ValueProviderContext { Random = Random, GetPreviousValue = () => GetValue(1, previousKey).Single() };
    }

    private void RaiseFillCount (Type currentType)
    {
      if (_typeFillCountDictionary.ContainsKey(currentType))
      {
        _typeFillCountDictionary[currentType]++;
      }
      else
      {
        _typeFillCountDictionary.Add(currentType, 1);
      }
    }
    
    private bool MayFill (Type currentType, int maxDepth)
    {
      if (maxDepth <= 0)
        return false;

      return !_typeFillCountDictionary.ContainsKey(currentType) || _typeFillCountDictionary[currentType] < maxDepth;
    }
  }
}