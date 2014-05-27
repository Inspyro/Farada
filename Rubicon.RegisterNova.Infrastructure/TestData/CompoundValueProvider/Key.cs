using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remotion.Utilities;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  // TODO: Maybe refactor to use polymorphism to express the chaining rules?
  [System.Diagnostics.DebuggerDisplay("{ToString()}")]
  public class Key // TODO : IEquatable<Key>
  {
    private readonly IList<KeyPart> _keyParts;
    private readonly Key _cachedPreviousKey;

    internal KeyPart Top { get; private set; }
    internal KeyPart Bottom { get; private set; }

    public int RecursionDepth { get; private set; }

    internal Key (Type propertyType, IFastPropertyInfo propertyInfo = null)
        : this(new List<KeyPart> { new KeyPart(propertyType, propertyInfo) })
    {
    }

    internal Key (IList<KeyPart> keyParts)
    {
      _keyParts = keyParts;
      Top = _keyParts.Last();
      Bottom = _keyParts.First();
      RecursionDepth = _keyParts.Count(part => part.PropertyType == Top.PropertyType);

      _cachedPreviousKey = CreatePreviousKey();
    }

    private Key CreatePreviousKey ()
    {
      var baseKey = GetBaseKey();
      if (baseKey != null)
        return baseKey;

      if (_keyParts.Count == 1 && _keyParts[0].Property != null)
        return new Key(_keyParts[0].PropertyType);

      var keyPartsOfPreviousKey = _keyParts.Slice(1);
      if (keyPartsOfPreviousKey.Count == 0)
        return null;

      keyPartsOfPreviousKey[0] = new KeyPart(keyPartsOfPreviousKey[0].PropertyType);

      return new Key(keyPartsOfPreviousKey);
    }

    private Key GetBaseKey ()
    {
      var baseType = Bottom.PropertyType.BaseType;
      if (baseType == typeof (object)|| baseType == typeof(ValueType))
        return null;

      var newBottomKeyPart = new KeyPart(baseType);
      var newKeyParts = new[]{newBottomKeyPart}.Concat(_keyParts.Skip(1)).ToList();
      return new Key(newKeyParts);
    }

    internal Key GetPreviousKey ()
    {
      return _cachedPreviousKey;
    }

    internal Key GetNextKey (IFastPropertyInfo property)
    {
      var keyPartsOfNextKey = new List<KeyPart>(_keyParts)
                              {
                                  new KeyPart(property.PropertyType, property)
                              };

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
      return EqualityUtility.GetRotatedHashCode(_keyParts);
    }

    public override string ToString ()
    {
      return "KEY: " + string.Join(" > ", _keyParts.Select(kp => kp.ToString()));
    }
  }

  internal class AttributeInfo
  {
    public Type AttributeType { get; private set; }
    public Attribute Attribute { get; private set; }

    internal AttributeInfo(Type attributeType, Attribute attribute=null)
    {
      AttributeType = attributeType;
      Attribute = attribute;
    }
  }
}