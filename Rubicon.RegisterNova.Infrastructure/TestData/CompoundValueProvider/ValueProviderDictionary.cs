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
    private readonly Dictionary<Key, ValueProviderLink> _valueProviders;

    public ValueProviderDictionary ()
    {
      _valueProviders = new Dictionary<Key, ValueProviderLink>(new KeyComparer());
    }

    internal void AddValueProvider (Key key, IValueProvider valueProvider)
    {
      if (!_valueProviders.ContainsKey(key))
      {
        _valueProviders[key] = new ValueProviderLink(valueProvider, key, () => GetLink(key.GetPreviousKey()));
      }
      else
      {
        var previousKey = _valueProviders[key];
        _valueProviders[key] = new ValueProviderLink(valueProvider, key, () => previousKey);
      }
    }

    private ValueProviderLink GetOrDefault (Key key)
    {
      return key!=null&&_valueProviders.ContainsKey(key) ? _valueProviders[key] : null;
    }

    internal ValueProviderLink GetLink (Key key)
    {
      ValueProviderLink link = null;
      while (link == null && key != null)
      {
        link = GetOrDefault(key);
        key = key.GetPreviousKey();
      }

      return link;
    }
  }

  internal class ValueProviderLink
  {
    internal IValueProvider Value { get; private set; }
    internal Key Key { get; private set; }
    internal Func<ValueProviderLink> Previous { get; private set; }

    internal ValueProviderLink (IValueProvider value, Key key, Func<ValueProviderLink> previous)
    {
      Value = value;
      Key = key;
      Previous = previous;
    }
  }
}