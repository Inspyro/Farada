using System;
using System.Collections.Generic;
using System.Dynamic;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  public class DependedPropertyCollection
  {
    private readonly Dictionary<IKey, object> _valueMapping;
    public DependedPropertyCollection()
    {
      _valueMapping = new Dictionary<IKey, object>();
    } 

    internal void Add(IKey dependencyKey, object value)
    {
      _valueMapping.Add (dependencyKey, value);
    }

    internal object this[IKey key] => _valueMapping[key];

    internal bool ContainsKey(IKey key)
    {
      return _valueMapping.ContainsKey (key);
    }
  }
}