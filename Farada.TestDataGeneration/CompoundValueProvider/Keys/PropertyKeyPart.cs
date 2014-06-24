using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.CompoundValueProvider.Keys
{
  /// <summary>
  ///  TODO
  /// </summary>
  internal class PropertyKeyPart: IEquatable<PropertyKeyPart>
  {
    public Type PropertyType { get; private set; }
    public IFastPropertyInfo Property { get; private set; }

    internal PropertyKeyPart (IFastPropertyInfo property, Type concreteType=null)
    {
      Property = property;
      PropertyType = concreteType ?? Property.PropertyType;
    }

    public override string ToString ()
    {
      return string.Format("PropertyType: {0}, Property: {1}", PropertyType.Name, Property.Name);
    }

    public bool Equals (PropertyKeyPart other)
    {
      if (!EqualityUtility.ClassEquals(this, other))
        return false;

      return Property.Name == other.Property.Name && PropertyType == other.PropertyType;
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
        hashCode = (hashCode * 397) ^ (PropertyType.GetHashCode());
        return hashCode;
      }
    }
  }
}