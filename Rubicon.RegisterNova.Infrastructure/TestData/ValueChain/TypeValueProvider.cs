using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class TypeValueProvider
  {
    private readonly IChainValueProvider _valueChain;
    internal TypeValueProvider(IChainValueProvider valueChain)
    {
      _valueChain = valueChain;
    }

    private Dictionary<Type, int> _typeFillCountDictionary;
    public TValue Get<TValue>(int maxDepth=2)
    {
      _typeFillCountDictionary = new Dictionary<Type, int>();

      var value = Get(_valueChain, typeof (TValue), null, maxDepth);
      return value == null ? default(TValue) : (TValue) value;
    }

  
    private object Get(IChainValueProvider currentChain, Type currentType, string currentFilter, int maxDepth)
    {
      IChainValueProvider directValueProvider = null;
      if(currentChain.HasChainProvider(currentType, currentFilter))
      {
        directValueProvider = currentChain.GetChainProvider(currentType, currentFilter);
      }

      var hasDirectValue = directValueProvider != null && directValueProvider.HasValue();

      //if we try to get a basic type (e.g. no class) we can only get the value directly...
      if (!currentType.IsClass||currentType==typeof(string))
      {
        return hasDirectValue ? directValueProvider.GetValue() : null;
      }

      var instance = hasDirectValue ? directValueProvider.GetValue() : Activator.CreateInstance(currentType);

      if(!MayFill(currentType, maxDepth))
      {
        return instance;
      }

      RaiseFillCount(currentType);

      //we go over all properties and generate values for them
      var properties = currentType.GetProperties();
      foreach (var property in properties)
      {
        //if there is no direct value provider we try to get all values from the base chain
        if (directValueProvider == null)
        {
          TryFillProperty(_valueChain, property, instance, maxDepth);
        }
        else //else we try to get all properties from the concrete chain
        {
          if (!TryFillProperty(directValueProvider, property, instance, maxDepth)) //if we cant fill the properties with the direct chain (current level) we get the values from the default chain
          {
            if(!TryFillProperty(currentChain, property, instance, maxDepth))
            {
              TryFillProperty(_valueChain, property, instance, maxDepth);
            }
          }
        }
      }

      return instance;
    }

    private void RaiseFillCount (Type currentType)
    {
      if(_typeFillCountDictionary.ContainsKey(currentType))
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

    private bool TryFillProperty (IChainValueProvider valueProvider, PropertyInfo property, object instance, int maxDepth)
    {
      if (CanFillProperty(valueProvider, property, true))
      {
        FillProperty(valueProvider, property, instance, true, maxDepth);
        return true;
      }

      if (CanFillProperty(valueProvider, property, false))
      {
        FillProperty(valueProvider, property, instance, false, maxDepth);
        return true;
      }

      return false;
    }

    private static bool CanFillProperty (IChainValueProvider valueProvider, PropertyInfo property, bool filterName)
    {
      return valueProvider.HasChainProvider(property.PropertyType, filterName ? property.Name : null);
    }

    private void FillProperty (IChainValueProvider directValueProvider, PropertyInfo property, object instance, bool filterName, int maxDepth)
    {
      var propertyValue = Get(directValueProvider, property.PropertyType, filterName?property.Name:null, maxDepth);
      property.SetValue(instance, propertyValue);
    }
  }
}