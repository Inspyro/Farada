using System;
using System.Collections.Generic;
using System.Linq;
using Farada.Core.Extensions;
using Farada.Core.FastReflection;

namespace Farada.Core.CompoundValueProvider.Keys
{
  internal class AttributeKey : IKey, IEquatable<AttributeKey>
  {
    private readonly Type _propertyType;
    private readonly Type _attributeType;

    private readonly Type _mostConcretePropertyType;
    private IList<Type> _remainingAttributes;

    internal AttributeKey(Type propertyType, Type attributeType)
      : this(propertyType, new List<Type> { attributeType})
    {
    }

    internal AttributeKey (Type propertyType, IList<Type> remainingAttributes):this(propertyType, propertyType, remainingAttributes)
    {
      
    }

    private AttributeKey(Type propertyType, Type mostConcretePropertyType, IList<Type> remainingAttributes )
    {
      if (remainingAttributes.Count < 1)
        throw new ArgumentException("Cannot create attribute key with less then one remaining attribute");

      _propertyType = propertyType;
      _mostConcretePropertyType = mostConcretePropertyType;
      _attributeType = remainingAttributes.First();
      _remainingAttributes = remainingAttributes;

      PreviousKey = CreatePreviousKey(remainingAttributes);
    }

    private IKey CreatePreviousKey (IList<Type> remainingAttributes)
    {
      if (remainingAttributes.Count == 1)
      {
        return new TypeKey(_mostConcretePropertyType);
      }

      if (_propertyType.BaseType != null)
        return new AttributeKey(_propertyType.BaseType, _propertyType, remainingAttributes);

      return new AttributeKey(_propertyType, _mostConcretePropertyType, remainingAttributes.Slice(1));
    }

    public IKey PreviousKey { get; private set; }

    public IKey CreateKey (IFastPropertyInfo property)
    {
      return new ChainedKey(_propertyType, new List<PropertyKeyPart> { new PropertyKeyPart(property) });
    }

    public Type PropertyType
    {
      get { return _propertyType; }
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

    public bool Equals (AttributeKey other)
    {
      if (!EqualityUtility.ClassEquals(this, other))
        return false;

      return _propertyType == other._propertyType && _attributeType == other._attributeType;
    }

    public bool Equals (IKey other)
    {
      return Equals(other as AttributeKey);
    }

    public override bool Equals (object obj)
    {
      return Equals(obj as AttributeKey);
    }

    public override int GetHashCode ()
    {
      var hashCode = (_attributeType.GetHashCode());
      hashCode = (hashCode * 397) ^ (_propertyType.GetHashCode());
      return hashCode;
    }
  }
}