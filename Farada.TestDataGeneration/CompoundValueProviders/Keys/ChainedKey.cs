using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;
using Remotion.Utilities;
using EqualityUtility = Farada.TestDataGeneration.Extensions.EqualityUtility;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
{
  /// <summary>
  /// A key that represents a type chain (Class.Property1.Property2...)
  /// </summary>
  internal class ChainedKey : IKey, IEquatable<ChainedKey>
  {
    private readonly Type _declaringType;
    private readonly IList<PropertyKeyPart> _propertyChain;

    private readonly PropertyKeyPart _lastProperty;
    private readonly Type _concreteDeclaringType;

    public IFastPropertyInfo Property
    {
      get { return _lastProperty.Property; }
    }

    public int RecursionDepth { get; private set; }

    public IKey ChangePropertyType (Type newPropertyType)
    {
      if (Property == null)
        throw new InvalidOperationException ("You cannot change the property type of a non-existing (null) property");

      var newPropertyChain = new List<PropertyKeyPart> (_propertyChain);
      newPropertyChain[newPropertyChain.Count - 1] = new PropertyKeyPart (Property, newPropertyType);

      return new ChainedKey (_declaringType, newPropertyChain);
    }

    public IKey PreviousKey { get; private set; }

    internal ChainedKey (Type declaringType, IFastPropertyInfo propertyInfo)
        : this (declaringType, new List<PropertyKeyPart> { new PropertyKeyPart (propertyInfo) })
    {
    }

    internal ChainedKey (Type declaringType, IList<PropertyKeyPart> propertyChain)
        : this (declaringType, declaringType, propertyChain)
    {
    }

    private ChainedKey (Type declaringType, Type concreteDeclaringType, IList<PropertyKeyPart> propertyChain)
    {
      ArgumentUtility.CheckNotNull ("declaringType", declaringType);

      _declaringType = declaringType;
      _concreteDeclaringType = concreteDeclaringType;
      _propertyChain = propertyChain;

      _lastProperty = propertyChain.Last();

      RecursionDepth = _propertyChain.Count (keyPart => keyPart.PropertyType == _lastProperty.PropertyType);
      PreviousKey = CreatePreviousKey();
    }

    private IKey CreatePreviousKey ()
    {
      var baseType = _declaringType.BaseType;
      if (baseType != typeof (object) && baseType != typeof (ValueType) && baseType != null)
        return new ChainedKey (baseType, _concreteDeclaringType, _propertyChain);

      var firstProperty = _propertyChain[0];
      var previousProperties = _propertyChain.Slice (1);

      if (previousProperties.Count == 0)
      {
        var attributes = firstProperty.Property.Attributes.ToList();
        return attributes.Count > 0
            ? (IKey) new AttributeKey (firstProperty.PropertyType, attributes)
            : new TypeKey (firstProperty.PropertyType);
      }

      var previousDeclaringType = firstProperty.PropertyType;
      return new ChainedKey (previousDeclaringType, previousProperties);
    }

    public IKey CreateKey (IFastPropertyInfo property)
    {
      return new ChainedKey (_declaringType, new List<PropertyKeyPart> (_propertyChain) { new PropertyKeyPart (property) });
    }

    public Type Type
    {
      get { return _lastProperty.PropertyType; }
    }

    public bool Equals ([CanBeNull] ChainedKey other)
    {
      if (!EqualityUtility.ClassEquals (this, other))
        return false;

      Trace.Assert (other != null);
      if (_declaringType != other._declaringType)
        return false;

      if (_propertyChain.Count != other._propertyChain.Count)
        return false;

      return !_propertyChain.Where ((t, i) => !t.Equals (other._propertyChain[i])).Any();
    }

    public override bool Equals ([CanBeNull] object obj)
    {
      return Equals (obj as ChainedKey);
    }

    public override int GetHashCode ()
    {
      return _declaringType.GetHashCode() ^ Remotion.Utilities.EqualityUtility.GetRotatedHashCode (_propertyChain);
    }

    public bool Equals ([CanBeNull] IKey other)
    {
      return Equals (other as ChainedKey);
    }

    public override string ToString ()
    {
      return "KEY on " + _declaringType + ": " + string.Join (" > ", _propertyChain.Select (kp => kp.ToString()));
    }
  }
}