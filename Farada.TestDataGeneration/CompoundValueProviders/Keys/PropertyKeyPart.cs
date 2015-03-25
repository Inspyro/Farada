using System;
using System.Diagnostics;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
{
  /// <summary>
  ///  A key that represents a single property
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

    public bool Equals ([CanBeNull] PropertyKeyPart other)
    {
      if (!EqualityUtility.ClassEquals(this, other))
        return false;

      Trace.Assert (other != null);
      return Property.Name == other.Property.Name && PropertyType == other.PropertyType;
    }

    public override bool Equals ([CanBeNull] object obj)
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