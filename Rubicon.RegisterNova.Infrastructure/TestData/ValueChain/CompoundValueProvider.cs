using System;
using System.Collections.Generic;
using System.Reflection;
using Rubicon.RegisterNova.Infrastructure.TestData.Reflection;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class CompoundValueProvider
  {
    public Random Random { get { return _valueChain.Random; } }

    private readonly IChainValueProvider _valueChain;

    internal CompoundValueProvider (IChainValueProvider valueChain)
    {
      _valueChain = valueChain;
    }

    private Dictionary<Type, int> _typeFillCountDictionary;
    public TValue Create<TValue>(int maxDepth=2)
    {
      _typeFillCountDictionary = new Dictionary<Type, int>();

      var value = Create(_valueChain, typeof (TValue), null, maxDepth);
      return value == null ? default(TValue) : (TValue) value;
    }

  
    private object Create(IChainValueProvider currentChain, Type currentType, string currentFilter, int maxDepth)
    {
      IChainValueProvider directValueProvider = null;
      if(currentChain.HasChainProvider(currentType, currentFilter))
      {
        directValueProvider = currentChain.GetChainProvider(currentType, currentFilter);
      }

      var hasDirectValue = directValueProvider != null && directValueProvider.HasValue();

      //if we try to get a basic type (e.g. no class) we can only get the value directly...
      if (!currentType.CanBeInstantiated())
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
      var properties = FastReflection.GetTypeInfo(currentType).Properties; //currentType.GetProperties();
      foreach (var property in properties)
      {
        //if there is no direct value provider we try to get all values from the base chain
        if (directValueProvider == null)
        {
          TryFillProperty(_valueChain, property, instance, maxDepth, true);
          continue;
        }

        if (TryFillProperty(directValueProvider, property, instance, maxDepth,false))
          continue;

        //if we cant fill the properties with the direct chain (current level) we get the values from the default chain
        if (TryFillProperty(currentChain, property, instance, maxDepth, false))
          continue;

        TryFillProperty(_valueChain, property, instance, maxDepth, true);
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

    private bool TryFillProperty (IChainValueProvider valueProvider, IFastPropertyInfo property, object instance, int maxDepth, bool fallback)
    {
      if (CanFillProperty(valueProvider, property, true, fallback))
      {
        FillProperty(valueProvider, property, instance, true, maxDepth);
        return true;
      }

      if (CanFillProperty(valueProvider, property, false, fallback))
      {
        FillProperty(valueProvider, property, instance, false, maxDepth);
        return true;
      }

      return false;
    }

    private static bool CanFillProperty (IChainValueProvider valueProvider, IFastPropertyInfo property, bool shouldFilterName, bool fallback)
    {
      //a non value type can at least be instantiated...
      return fallback&&property.PropertyType.CanBeInstantiated() || valueProvider.HasChainProvider(property.PropertyType, shouldFilterName ? property.Name : null);
    }

    private void FillProperty (IChainValueProvider directValueProvider, IFastPropertyInfo property, object instance, bool shouldFilterName, int maxDepth)
    {
      var propertyValue = Create(directValueProvider, property.PropertyType, shouldFilterName?property.Name:null, maxDepth);
      property.SetValue(instance, propertyValue);
    }
  }
}