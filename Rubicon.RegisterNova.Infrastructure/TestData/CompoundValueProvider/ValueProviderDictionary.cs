using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider.Keys;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  internal class ValueProviderDictionary
  {
    private readonly Dictionary<IKey, ValueProviderLink> _valueProviders;

    public ValueProviderDictionary ()
    {
      _valueProviders = new Dictionary<IKey, ValueProviderLink>();
    }

    internal void AddValueProvider (IKey key, IValueProvider valueProvider)
    {
      if (!_valueProviders.ContainsKey(key))
      {
        _valueProviders[key] = new ValueProviderLink(valueProvider, key, () => GetLink(key.PreviousKey));
      }
      else
      {
        var previousKey = _valueProviders[key];
        _valueProviders[key] = new ValueProviderLink(valueProvider, key, () => previousKey);
      }
    }

    private ValueProviderLink GetOrDefault (IKey key)
    {
      return key!=null&&_valueProviders.ContainsKey(key) ? _valueProviders[key] : null;
    }

    internal ValueProviderLink GetLink (IKey key)
    {
      ValueProviderLink link = null;
      var concreteType = key.PropertyType;

      while ((link == null||link.Value==null||!link.Value.CanHandle(concreteType)) && key != null)
      {
        link = GetOrDefault(key);
        key = key.PreviousKey;
      }

      return link;
    }
  }
}