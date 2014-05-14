using System;
using System.Collections.Generic;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  internal class ValueProviderDictionary
  {
    private readonly Dictionary<Key, IValueProvider> _valueProviders;

    public ValueProviderDictionary ()
    {
      _valueProviders = new Dictionary<Key, IValueProvider>(new KeyComparer());
    }

    internal void SetValueProvider (Key key, IValueProvider valueProvider)
    {
      _valueProviders[key] = valueProvider;
    }

    internal IValueProvider GetValueProviderFor (Key currentKey)
    {
      while (currentKey != null)
      {
        if (_valueProviders.ContainsKey(currentKey))
          return _valueProviders[currentKey];

        currentKey = currentKey.GetPreviousKey();
      }

      return null;
    }
  }
}