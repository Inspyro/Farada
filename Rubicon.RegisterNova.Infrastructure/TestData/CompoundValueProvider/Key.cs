using System;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  internal class Key
  {
    private readonly IList<KeyPart> _keyParts;
    private readonly Key _cachedPreviousKey;

    public Type Type { get; private set; }
    public int RecursionDepth { get; private set; }

    internal Key (Type propertyType, string propertyName = null)
        : this(new List<KeyPart> { new KeyPart(propertyType, propertyName) })
    {
    }

    internal Key (IList<KeyPart> keyParts)
    {
      _keyParts = keyParts;
      _cachedPreviousKey = CreatePreviousKey();

      Type = _keyParts.Last().PropertyType;
      RecursionDepth = _keyParts.Count(part => part.PropertyType == Type);
    }

    private Key CreatePreviousKey ()
    {
      var keyPartsOfPreviousKey = _keyParts.Slice(1);
      if (keyPartsOfPreviousKey.Count == 0)
        return null;

      keyPartsOfPreviousKey[0] = new KeyPart(keyPartsOfPreviousKey[0].PropertyType);

      return new Key(keyPartsOfPreviousKey);
    }

    internal Key GetPreviousKey ()
    {
      return _cachedPreviousKey;
    }

    public Key GetNextKey (Type propertyType, string propertyName)
    {
      var keyPartsOfNextKey = new List<KeyPart>(_keyParts) { new KeyPart(propertyType, propertyName) };
      return new Key(keyPartsOfNextKey);
    }

    // REVIEW FS: Should we keep KeyComparer as well as this Equals()/GetHashCode() implementation?
    private bool Equals (Key other)
    {
      if (_keyParts.Count != other._keyParts.Count)
        return false;

      return !_keyParts.Where((t, i) => !t.Equals(other._keyParts[i])).Any();
    }

    public override bool Equals (object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      if (ReferenceEquals(this, obj))
        return true;
      if (obj.GetType() != GetType())
        return false;
      return Equals((Key) obj);
    }

    public override int GetHashCode ()
    {
      return _keyParts.Aggregate(0, (current, value) => current ^ value.GetHashCode());
    }
  }
}