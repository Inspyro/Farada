using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.ValueProviders;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// Stores all value providers in a dictionary where they can be accessed in a fast manner
  /// </summary>
  internal class ValueProviderDictionary
  {
    private readonly Dictionary<IKey, ValueProviderLink> _valueProviders;

    internal ValueProviderDictionary ()
    {
      _valueProviders = new Dictionary<IKey, ValueProviderLink>();
    }

    internal void AddValueProvider (IKey key, IValueProvider valueProvider)
    {
      if (!_valueProviders.ContainsKey (key))
      {
        _valueProviders[key] = new ValueProviderLink (valueProvider, key, () => key.PreviousKey == null ? null : GetLink (key.PreviousKey));
      }
      else
      {
        var previousKey = _valueProviders[key];
        _valueProviders[key] = new ValueProviderLink (valueProvider, key, () => previousKey);
      }
    }

   
    [CanBeNull]
    private ValueProviderLink GetOrDefault ([CanBeNull] IKey key)
    {
      return key!=null&&_valueProviders.ContainsKey(key) ? _valueProviders[key] : null;
    }

    /// <summary>
    /// Returns true if the given type is filled by a value provider already (so auto-filling can be skipped).
    /// </summary>
    internal bool IsTypeFilledByValueProvider(IKey key)
    {
      var currentKey = key;
      while (currentKey != null)
      {
        var link = GetOrDefault (currentKey);
        if (link?.Value!=null)
        {
          if (link.Value.FillsType(key.Type))
            return true; //we have no direct value provider link, so we will try to auto-fill the key.
        }

        currentKey = currentKey.PreviousKey;
      }
      return false;
    }

    [CanBeNull]
    internal ValueProviderLink GetLink (IKey key)
    {
      ValueProviderLink link = null;
      var concreteType = key.Type;

      while ((link?.Value == null || !link.Value.CanHandle(concreteType)) && key != null)
      {
        link = GetOrDefault(key);
        key = key.PreviousKey;
      }

      return link;
    }
  }
}