using System;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.Reflection;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class CompoundValueProvider
  {
    public Random Random
    {
      get { return _baseValueProvider.Random; }
    }

    private readonly IChainValueProvider _baseValueProvider;

    internal CompoundValueProvider (IChainValueProvider baseValueProvider)
    {
      _baseValueProvider = baseValueProvider;
    }

    private Dictionary<Type, int> _typeFillCountDictionary;

    public TValue Create<TValue> (int maxDepth = 2)
    {
      _typeFillCountDictionary = new Dictionary<Type, int>();

      var baseStack = new List<IChainValueProvider>();
      baseStack.Add(_baseValueProvider);

      var value = Create(baseStack, typeof (TValue), null, maxDepth, null);
      return value == null ? default(TValue) : (TValue) value;
    }


    private object Create (IList<IChainValueProvider> chainValueProviders, Type currentType, string currentFilter, int maxDepth, object currentValue)
    {
      IChainValueProvider directValueProvider = null;
      var currentValueProvider = chainValueProviders.First();
      if (currentValueProvider.HasChainProvider(currentType, currentFilter))
      {
        directValueProvider = currentValueProvider.GetChainProvider(currentType, currentFilter);

        var newChainValueProviders = new List<IChainValueProvider> { directValueProvider };
        newChainValueProviders.AddRange(chainValueProviders);

        chainValueProviders = newChainValueProviders;
      }

      var hasDirectValue = directValueProvider != null && directValueProvider.HasValue();

      //if we try to get a basic type (e.g. no class) we can only get the value directly...
      if (!currentType.CanBeInstantiated())
      {
        return hasDirectValue ? directValueProvider.GetValue(currentValue) : null;
      }

      var instance = hasDirectValue ? directValueProvider.GetValue(currentValue) : Activator.CreateInstance(currentType);

      if (!MayFill(currentType, maxDepth))
      {
        return instance;
      }

      RaiseFillCount(currentType);

      //we go over all properties and generate values for them
      var properties = FastReflection.GetTypeInfo(currentType).Properties;
      foreach (var property in properties)
      {
        var currentProviders = Sort(chainValueProviders, property);
        while (currentProviders.Count > 0)
        {
          var isLastPossibility = currentProviders.Count == 1;

          if (TryFillProperty(currentProviders, property, instance, maxDepth, isLastPossibility))
            break;

          currentProviders = currentProviders.Slice(1);
        }
      }

      return instance;
    }

    private static IList<IChainValueProvider> Sort (IList<IChainValueProvider> chainValueProviders, IFastPropertyInfo property)
    {
      var sortedList = new List<IChainValueProvider>();
      for (var i = 0; i < chainValueProviders.Count; i++)
      {
        var currentProvider = chainValueProviders[i];

        if (i < chainValueProviders.Count - 1 && WantsValue(currentProvider, property))
        {
          var previousProvider = chainValueProviders[i + 1];
          var targetIndex = i == 0 ? 0 : chainValueProviders.IndexOf(currentProvider); //TODO: maybe use o(1) version of list
          sortedList.Insert(targetIndex, previousProvider);
        }

        sortedList.Add(currentProvider);
      }

      return sortedList;
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

    private bool TryFillProperty (IList<IChainValueProvider> valueProviders, IFastPropertyInfo property, object instance, int maxDepth, bool fallback)
    {
      var targetProvider = valueProviders.First();
      if (CanFillProperty(targetProvider, property, true, false))
      {
        FillProperty(valueProviders, property, instance, true, maxDepth);
        return !NextWantsValue(valueProviders, property);
      }

      if (CanFillProperty(targetProvider, property, false, fallback))
      {
        FillProperty(valueProviders, property, instance, false, maxDepth);
        return !NextWantsValue(valueProviders, property);
      }

      return false;
    }

    private static bool NextWantsValue (IList<IChainValueProvider> valueProviders, IFastPropertyInfo property)
    {
      if (valueProviders.Count < 2)
        return false;

      var targetProvider = valueProviders[1];
      return WantsValue(targetProvider, property);
    }

    private static bool WantsValue (IChainValueProvider valueProvider, IFastPropertyInfo property)
    {
      return WantsValue(valueProvider, property, true) || WantsValue(valueProvider, property, false);
    }

    private static bool WantsValue (IChainValueProvider valueProvider, IFastPropertyInfo property, bool shouldFilterName)
    {
      //a non value type can at least be instantiated...
      return valueProvider.HasChainProvider(property.PropertyType, shouldFilterName ? property.Name : null)
             && valueProvider.GetChainProvider(property.PropertyType, shouldFilterName ? property.Name : null).WantsPreviousValue();
    }

    private static bool CanFillProperty (IChainValueProvider valueProvider, IFastPropertyInfo property, bool shouldFilterName, bool fallback)
    {
      //a non value type can at least be instantiated...
      return fallback && property.PropertyType.CanBeInstantiated()
             || valueProvider.HasChainProvider(property.PropertyType, shouldFilterName ? property.Name : null);
    }

    private void FillProperty (
        IList<IChainValueProvider> valueProviders,
        IFastPropertyInfo property,
        object instance,
        bool shouldFilterName,
        int maxDepth)
    {
      var value=property.GetValue(instance);
      var propertyValue = Create(valueProviders, property.PropertyType, shouldFilterName ? property.Name : null, maxDepth, value);
      property.SetValue(instance, propertyValue);
    }
  }
}