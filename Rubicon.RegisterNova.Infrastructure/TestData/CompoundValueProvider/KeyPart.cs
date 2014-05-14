using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  ///  TODO
  /// </summary>
  internal class KeyPart
  {
    internal Type PropertyType { get; private set; }
    internal string PropertyName { get; private set; }

    internal KeyPart (Type propertyType, string propertyName = null)
    {
      PropertyType = propertyType;
      PropertyName = propertyName;
    }

    // REVIEW FS: Should we keep KeyComparer as well as this Equals()/GetHashCode() implementation?
    private bool Equals (KeyPart other)
    {
      return string.Equals(PropertyName, other.PropertyName) && PropertyType == other.PropertyType;
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
        return ((PropertyName != null ? PropertyName.GetHashCode() : 0) * 397) ^ (PropertyType != null ? PropertyType.GetHashCode() : 0);
      }
    }
  }
}