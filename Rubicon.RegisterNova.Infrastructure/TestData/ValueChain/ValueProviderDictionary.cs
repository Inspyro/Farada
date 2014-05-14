using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  internal class Key
  {
    private readonly IList<KeyPart> _values;
    private readonly Key _cachedPreviousKey;
    
    private static readonly Dictionary<Tuple<Key, Type, string>, Key> _cachedNextKeys = new Dictionary<Tuple<Key, Type, string>, Key>();

    internal Key (KeyPart part)
        : this(new List<KeyPart> { part })
    {
    }

    internal Key (IList<KeyPart> values)
    {
      _values = values;
      _cachedPreviousKey = CreatePreviousKey();
    }

    private Key CreatePreviousKey ()
    {
      var previousValues = _values.Slice(1);
      if (previousValues.Count == 0)
        return null;

      previousValues[0] = new KeyPart(previousValues[0].PropertyType);

      return new Key(previousValues);
    }

    internal Key GetPreviousKey ()
    {
      return _cachedPreviousKey;
    }

    public Key GetNextKey (Type propertyType, string name)
    {
      var cacheKey = new Tuple<Key, Type, string>(this, propertyType, name);

      if (!_cachedNextKeys.ContainsKey(cacheKey))
      {
        var values = new List<KeyPart>(_values) { new KeyPart(propertyType, name) };
        _cachedNextKeys.Add(cacheKey, new Key(values));
      }

      return _cachedNextKeys[cacheKey];
    }

    public Type GetTopType ()
    {
      return _values.Last().PropertyType;
    }

    protected bool Equals (Key other)
    {
      if (_values.Count != other._values.Count)
        return false;

      for (int i = 0; i < _values.Count; ++i)
      {
        if (!_values[i].Equals(other._values[i]))
          return false;
      }

      return true;
    }

    public override bool Equals (object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      if (ReferenceEquals(this, obj))
        return true;
      if (obj.GetType() != this.GetType())
        return false;
      return Equals((Key) obj);
    }

    public override int GetHashCode ()
    {
      int hash = 0;
      foreach (var value in _values)
        hash ^= value.GetHashCode();
      return hash;
    }
  }

  internal class KeyPart
  {
    internal Type PropertyType { get; private set; }
    internal string PropertyName { get; private set; }

    public KeyPart (Type propertyType, string propertyName = null)
    {
      PropertyType = propertyType;
      PropertyName = propertyName;
    }

    protected bool Equals (KeyPart other)
    {
      return string.Equals(PropertyName, other.PropertyName) && PropertyType == other.PropertyType;
    }

    public override bool Equals (object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      if (ReferenceEquals(this, obj))
        return true;
      if (obj.GetType() != this.GetType())
        return false;
      return Equals((KeyPart) obj);
    }

    public override int GetHashCode ()
    {
      unchecked
      {
        return ((PropertyName != null ? PropertyName.GetHashCode() : 0) * 397) ^ (PropertyType != null ? PropertyType.GetHashCode() : 0);
      }
    }
  }

  public class ValueProviderDictionary
  {
    private readonly Dictionary<Key, IValueProvider> _valueProviders;

    public ValueProviderDictionary ()
    {
      _valueProviders = new Dictionary<Key, IValueProvider>();
    }

    internal void SetValueProvider (Key key, IValueProvider valueProvider)
    {
      if (!_valueProviders.ContainsKey(key))
      {
        _valueProviders.Add(key, valueProvider);
      }
      else
      {
        _valueProviders[key] = valueProvider;
      }
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