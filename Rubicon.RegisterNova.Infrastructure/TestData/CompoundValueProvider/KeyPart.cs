using System;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  ///  TODO
  /// </summary>
  internal class KeyPart
  {
    internal Type PropertyType { get; private set; }
    internal IFastPropertyInfo Property { get; private set; }
    private readonly string _propertyName;

    internal KeyPart (Type propertyType, IFastPropertyInfo property=null)
    {
      ArgumentUtility.CheckNotNull ("propertyType", propertyType);

      PropertyType = propertyType;
      Property = property;

      _propertyName = Property != null ? Property.Name : null;
    }

    // REVIEW FS: Should we keep KeyComparer as well as this Equals()/GetHashCode() implementation?
    private bool Equals (KeyPart other)
    {
      return string.Equals(_propertyName, other._propertyName) && PropertyType == other.PropertyType;
    }

    public override bool Equals (object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      if (ReferenceEquals(this, obj))
        return true;
      if (obj.GetType() != GetType())
        return false;
      return Equals((KeyPart) obj);
    }

    public override int GetHashCode ()
    {
      unchecked
      {
        return ((_propertyName != null ? _propertyName.GetHashCode() : 0) * 397) ^ (PropertyType != null ? PropertyType.GetHashCode() : 0);
      }
    }
  }
}