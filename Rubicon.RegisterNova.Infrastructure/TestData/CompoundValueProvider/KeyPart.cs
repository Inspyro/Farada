using System;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  ///  TODO
  /// </summary>
  [System.Diagnostics.DebuggerDisplay("{ToString()}")]
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
    protected bool Equals (KeyPart other)
    {
      if (!string.Equals(_propertyName, other._propertyName))
        return false;

      if (PropertyType != other.PropertyType)
        return false;

      return true;
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
        var hashCode = (_propertyName != null ? _propertyName.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (PropertyType != null ? PropertyType.GetHashCode() : 0);
        return hashCode;
      }
    }

    public override string ToString ()
    {
      return string.Format("PropertyType: {0}, Property: {1}", PropertyType.Name, Property != null ? Property.Name : "null");
    }
  }
}