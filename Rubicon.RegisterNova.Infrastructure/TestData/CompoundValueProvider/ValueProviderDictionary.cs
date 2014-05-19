using System;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  internal class ValueProviderDictionary
  {
    private readonly Dictionary<Key, Stack<IValueProvider>> _valueProviders;

    public ValueProviderDictionary ()
    {
      _valueProviders = new Dictionary<Key, Stack<IValueProvider>>(new KeyComparer());
    }

    internal void AddValueProvider (Key key, IValueProvider valueProvider)
    {
      if(!_valueProviders.ContainsKey(key))
      {
        _valueProviders[key] = new Stack<IValueProvider>();
      }

      _valueProviders[key].Push(valueProvider);
    }

    internal IValueProvider GetValueProviderFor (Key currentKey, IValueProvider valueProviderToExclude=null)
    {
      while (currentKey != null)
      {
        var valueProvider = GetValueProvider(currentKey, valueProviderToExclude);
        if (valueProvider != null)
        {
          return valueProvider;
        }

        valueProvider = GetValueProvider(currentKey.WithouthAttributes(), valueProviderToExclude);

        if (valueProvider != null)
        {
          return valueProvider;
        }

        currentKey = currentKey.GetPreviousKey();
      }

      return null;
    }

    private IValueProvider GetValueProvider (Key currentKey, IValueProvider valueProviderToExclude)
    {
      return _valueProviders.ContainsKey(currentKey)
          ? _valueProviders[currentKey].FirstOrDefault(provider => provider != valueProviderToExclude)
          : null;
    }
  }
}