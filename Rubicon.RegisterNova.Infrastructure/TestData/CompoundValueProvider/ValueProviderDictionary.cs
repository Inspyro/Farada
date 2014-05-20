using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
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
      if (!_valueProviders.ContainsKey(currentKey))
        return null;

      var stack = _valueProviders[currentKey];

      if (valueProviderToExclude == null || !stack.Contains(valueProviderToExclude))
        return stack.First();

      return stack.SkipWhile(valueProvider => valueProvider != valueProviderToExclude).Skip(1).FirstOrDefault();
    }
  }
}