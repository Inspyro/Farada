using System;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider.Keys
{
  /// <summary>
  ///  TODO
  /// </summary>
  internal class PropertyKeyPart: IEquatable<PropertyKeyPart>
  {
    public IFastPropertyInfo Property { get; private set; }

    internal PropertyKeyPart (IFastPropertyInfo property)
    {
      Property = property;
    }

    public override string ToString ()
    {
      return string.Format("PropertyType: {0}, Property: {1}", Property.PropertyType.Name, Property.Name);
    }

    public bool Equals (PropertyKeyPart other)
    {
      if (!Utilities.EqualityUtility.ClassEquals(this, other))
        return false;

      return Property.Name == other.Property.Name && Property.PropertyType == other.Property.PropertyType;
    }

    public override bool Equals (object obj)
    {
      return Equals(obj as PropertyKeyPart);
    }

    public override int GetHashCode ()
    {
      unchecked
      {
        var hashCode = (Property.Name.GetHashCode());
        hashCode = (hashCode * 397) ^ (Property.PropertyType.GetHashCode());
        return hashCode;
      }
    }
  }
}