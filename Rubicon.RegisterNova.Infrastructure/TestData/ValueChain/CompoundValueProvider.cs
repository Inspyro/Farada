using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AutoMapper.Impl;
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
      _typeFillCountDictionary = new Dictionary<Type, int>();

      var key = new Key(new KeyPart(typeof (TValue)));
      var value = Create(key, maxDepth);
      return value == null ? default(TValue) : (TValue) value;
    }

    private object Create (Key currentKey, int maxDepth)
    {
      object instance = null;

      var valueProvider = _dictionary.GetValueProviderFor(currentKey);
      if(valueProvider!=null)
      {
        instance = GetValue(currentKey);
      }
      else if (currentKey.GetTopType().CanBeInstantiated())
      {
        instance = Activator.CreateInstance(currentKey.GetTopType());
      }

      if (instance == null || !MayFill(currentKey.GetTopType(), maxDepth))
        return instance;

      RaiseFillCount(currentKey.GetTopType());

      if (currentKey.GetTopType().IsCompoundType())
      {
        //we go over all properties and generate values for them
        var properties = FastReflection.GetTypeInfo(currentKey.GetTopType()).Properties;
        foreach (var property in properties)
        {
          var nextKey = currentKey.GetNextKey(property.PropertyType, property.Name);

          var propertyValue = Create(nextKey, maxDepth);
          if(propertyValue != null)
            property.SetValue(instance, propertyValue);
        }
      }

      return instance;
    }

    private object GetValue (Key key)
    {
      if (key == null)
        return null;

      var previousValueProvider = _dictionary.GetValueProviderFor(key);
      if (previousValueProvider == null)
        return null;

      var previousContext = CreateValueProviderContext(key);
      return previousValueProvider.GetObjectValue(previousContext);
    }

    private ValueProviderContext CreateValueProviderContext (Key key)
    {
      var previousKey = key.GetPreviousKey();
      return new ValueProviderContext { Random = Random, GetPreviousValue = () => GetValue(previousKey) };
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