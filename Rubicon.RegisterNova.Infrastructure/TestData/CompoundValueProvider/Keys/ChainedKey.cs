using System;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider.Keys
{
  internal class ChainedKey : IKey, IEquatable<ChainedKey>
  {
    private readonly Type _declaringType;
    private readonly IList<PropertyKeyPart> _propertyChain;

    private readonly PropertyKeyPart _top;

    public IFastPropertyInfo Property
    {
      get { return _top.Property; }
    }

    public int RecursionDepth { get; private set; }

    public IKey PreviousKey { get; private set; }

    internal ChainedKey(Type declaringType, IFastPropertyInfo propertyInfo)
      : this(declaringType, new List<PropertyKeyPart> { new PropertyKeyPart(propertyInfo)})
    {
    }

    internal ChainedKey (Type declaringType, IList<PropertyKeyPart> propertyChain)
    {
      ArgumentUtility.CheckNotNull("declaringType", declaringType);

      _declaringType = declaringType;
      _propertyChain = propertyChain;

      _top = propertyChain.Last();

      RecursionDepth = _propertyChain.Count(keyPart => keyPart.Property.PropertyType == _top.Property.PropertyType);
      PreviousKey = CreatePreviousKey();
    }

    private IKey CreatePreviousKey ()
    {
      var baseType = _declaringType.BaseType;
      if (baseType != typeof (object) && baseType != typeof (ValueType)&&baseType!=null)
        return new ChainedKey(baseType, _propertyChain);

      var bottomProperty = _propertyChain[0];
      var previousProperties = _propertyChain.Slice(1);

      if (previousProperties.Count == 0)
      {
        var attributes = bottomProperty.Property.Attributes.ToList();
        return attributes.Count > 0
            ? (IKey) new AttributeKey(bottomProperty.Property.PropertyType, attributes)
            : new TypeKey(bottomProperty.Property.PropertyType);
      }

      var previousDeclaringType = bottomProperty.Property.PropertyType;
      return new ChainedKey(previousDeclaringType, previousProperties);
    }

    public IKey CreateKey (IFastPropertyInfo property)
    {
      return new ChainedKey(_declaringType, new List<PropertyKeyPart>(_propertyChain) { new PropertyKeyPart(property) });
    }

    public Type PropertyType
    {
      get { return _top.Property.PropertyType; }
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