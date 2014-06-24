using System;
using System.Collections.Generic;
using System.Linq;
using Farada.Core.FastReflection;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Farada.Core.CompoundValueProvider.Keys
{
  internal class ChainedKey : IKey, IEquatable<ChainedKey>
  {
    private readonly Type _declaringType;
    private readonly IList<PropertyKeyPart> _propertyChain;

    private readonly PropertyKeyPart _top;
    private Type _concreteDeclaringType;

    public IFastPropertyInfo Property
    {
      get { return _top.Property; }
    }

    public int RecursionDepth { get; private set; }

    public IKey ChangePropertyType (Type newPropertyType)
    {
      var newPropertyChain = new List<PropertyKeyPart>(_propertyChain);
      newPropertyChain[newPropertyChain.Count - 1] = new PropertyKeyPart(Property, newPropertyType);

      return new ChainedKey(_declaringType, newPropertyChain);
    }

    public IKey PreviousKey { get; private set; }

    internal ChainedKey(Type declaringType, IFastPropertyInfo propertyInfo)
      : this(declaringType, new List<PropertyKeyPart> { new PropertyKeyPart(propertyInfo)})
    {
    }

    internal ChainedKey (Type declaringType, IList<PropertyKeyPart> propertyChain):this(declaringType, declaringType, propertyChain)
    {
      
    }

    private ChainedKey(Type declaringType, Type concreteDeclaringType, IList<PropertyKeyPart> propertyChain)
    {
      ArgumentUtility.CheckNotNull("declaringType", declaringType);

      _declaringType = declaringType;
      _concreteDeclaringType = concreteDeclaringType;
      _propertyChain = propertyChain;

      _top = propertyChain.Last();

      RecursionDepth = _propertyChain.Count(keyPart => keyPart.PropertyType == _top.PropertyType);
      PreviousKey = CreatePreviousKey();
    }

    private IKey CreatePreviousKey ()
    {
      var baseType = _declaringType.BaseType;
      if (baseType != typeof (object) && baseType != typeof (ValueType)&&baseType!=null)
        return new ChainedKey(baseType, _concreteDeclaringType, _propertyChain);

      var bottomProperty = _propertyChain[0];
      var previousProperties = _propertyChain.Slice(1);

      if (previousProperties.Count == 0)
      {
        var attributes = bottomProperty.Property.Attributes.ToList();
        return attributes.Count > 0
            ? (IKey) new AttributeKey(bottomProperty.PropertyType, attributes)
            : new TypeKey(bottomProperty.PropertyType);
      }

      var previousDeclaringType = bottomProperty.PropertyType;
      return new ChainedKey(previousDeclaringType, previousProperties);
    }

    public IKey CreateKey (IFastPropertyInfo property)
    {
      return new ChainedKey(_declaringType, new List<PropertyKeyPart>(_propertyChain) { new PropertyKeyPart(property) });
    }

    public Type PropertyType
    {
      get { return _top.PropertyType; }
    }

    public bool Equals (ChainedKey other)
    {
      if (!EqualityUtility.ClassEquals(this, other))
        return false;

      if (_declaringType != other._declaringType)
        return false;

      if (_propertyChain.Count != other._propertyChain.Count)
        return false;

      return _declaringType == other._declaringType && !_propertyChain.Where((t, i) => !t.Equals(other._propertyChain[i])).Any();
    }

    public override bool Equals (object obj)
    {
      return Equals(obj as ChainedKey);
    }

    public override int GetHashCode ()
    {
      return _declaringType.GetHashCode() ^ Remotion.Utilities.EqualityUtility.GetRotatedHashCode(_propertyChain);
    }

    public bool Equals (IKey other)
    {
      return Equals(other as ChainedKey);
    }

    public override string ToString ()
    {
      return "KEY on " + _declaringType + ": " + string.Join(" > ", _propertyChain.Select(kp => kp.ToString()));
    }
  }
}