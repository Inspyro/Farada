using System;
using System.Linq;
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
    internal Type[] Attributes { get; private set; }

    private readonly string _propertyName;

    internal KeyPart (Type propertyType, IFastPropertyInfo property=null, params Type[] attributes)
    {
      ArgumentUtility.CheckNotNull ("propertyType", propertyType);

      PropertyType = propertyType;
      Property = property;
      Attributes = attributes;

      _propertyName = Property != null ? Property.Name : null;
    }

    // REVIEW FS: Should we keep KeyComparer as well as this Equals()/GetHashCode() implementation?
    protected bool Equals (KeyPart other)
    {
      if (!string.Equals(_propertyName, other._propertyName))
        return false;

      if (PropertyType != other.PropertyType)
        return false;

      if (Attributes == other.Attributes)
        return true;

      if (Attributes.Length == 0 && other.Attributes.Length == 0)
        return true;

      if (Attributes == null || other.Attributes == null)
        return false;

      if(Attributes.Length==1)
      {
        return other.Attributes.Contains(Attributes[0]);
      }
      
      if (other.Attributes.Length == 1)
      {
        return Attributes.Contains(other.Attributes[0]);
      }

      return false;
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
        hashCode = (hashCode * 397) ^ (Attributes != null && Attributes.Length>0 ? 100 : 0);
        return hashCode;
      }
    }

  }
}