using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
{
  /// <summary>
  /// A key that represnets an attribute on a property
  /// </summary>
  internal class AttributeKey : IKey, IEquatable<AttributeKey>
  {
    private readonly Type _type;
    private readonly Type _attributeType;

    private readonly Type _mostConcretePropertyType;
    private readonly List<Type> _remainingAttributes;

    internal AttributeKey(Type type, Type attributeType)
      : this(type, new List<Type> { attributeType})
    {
    }

    internal AttributeKey(Type type, List<Type> remainingAttributes)
        : this(type, type, remainingAttributes)
    {
      
    }

    private AttributeKey(Type type, Type mostConcretePropertyType, List<Type> remainingAttributes )
    {
      if (remainingAttributes.Count < 1)
        throw new ArgumentException("Cannot create attribute key with less then one remaining attribute");

        remainingAttributes.Sort (new AlphaNumericComparer());

      _type = type;
      _mostConcretePropertyType = mostConcretePropertyType;
      _attributeType = remainingAttributes.First();
      _remainingAttributes = remainingAttributes;

      PreviousKey = CreatePreviousKey(remainingAttributes);
    }

    private IKey CreatePreviousKey(List<Type> remainingAttributes)
    {
      if (remainingAttributes.Count == 1)
      {
        return new TypeKey(_mostConcretePropertyType);
      }

      if (_type.BaseType != null)
        return new AttributeKey(_type.BaseType, _mostConcretePropertyType, remainingAttributes);

      return new AttributeKey(_mostConcretePropertyType, _mostConcretePropertyType, remainingAttributes.Slice(1));
    }

    public IKey PreviousKey { get; private set; }

    public IKey CreateKey (IFastPropertyInfo property)
    {
      return new ChainedKey(_type, new List<PropertyKeyPart> { new PropertyKeyPart(property) });
    }

    public Type Type
    {
      get { return _type; }
    }

    public IFastPropertyInfo Property
    {
      get { return null; }
    }

    public int RecursionDepth
    {
      get { return 0; }
    }

    public IKey ChangePropertyType (Type newPropertyType)
    {
      return new AttributeKey(newPropertyType, _remainingAttributes);
    }

    public bool Equals ( [CanBeNull] AttributeKey other)
    {
      if (!EqualityUtility.ClassEquals(this, other))
        return false;

      Trace.Assert (other != null);
      return _type == other._type && _attributeType == other._attributeType;
    }

    public bool Equals ([CanBeNull] IKey other)
    {
      return Equals(other as AttributeKey);
    }

    public override bool Equals ([CanBeNull] object obj)
    {
      return Equals(obj as AttributeKey);
    }

    public override int GetHashCode ()
    {
      var hashCode = (_attributeType.GetHashCode());
      hashCode = (hashCode * 397) ^ (_type.GetHashCode());
      return hashCode;
    }
  }
}