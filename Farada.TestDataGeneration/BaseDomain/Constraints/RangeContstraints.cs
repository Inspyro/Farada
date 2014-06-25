using System;
using System.ComponentModel.DataAnnotations;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.BaseDomain.Constraints
{
  /// <summary>
  /// Defines range constraints for a type, that is usually numeric
  /// </summary>
  /// <typeparam name="T">the type of the constraints</typeparam>
  public class RangeContstraints<T> where T:IComparable //TODO: Unit test?
  {
    /// <summary>
    /// The minimum value that is allowed
    /// </summary>
    public T MinValue { get; private set; }

    /// <summary>
    /// The maximum value that is allowed
    /// </summary>
    public T MaxValue { get; private set; }

    public RangeContstraints(T minValue, T maxValue)
    {
      MinValue = minValue;
      MaxValue = maxValue;
    }

    /// <summary>
    /// Reads the <see cref="RangeAttribute"/> from an <see cref="IFastPropertyInfo"/> and creates a new RangeConstraints object with type from the <see cref="IFastPropertyInfo"/>
    /// </summary>
    /// <param name="property">The property where for which the range constraints should be extracted</param>
    /// <returns></returns>
    public static RangeContstraints<T> FromProperty (IFastPropertyInfo property)
    {
      if (property == null || !property.IsDefined(typeof (RangeAttribute)))
        return null;

      var rangeAttribute = property.GetCustomAttribute<RangeAttribute>();

      if (rangeAttribute.OperandType != typeof (T))
        return null;

      if (((T) rangeAttribute.Minimum).CompareTo((T) rangeAttribute.Maximum) > 0)
      {
        throw new ArgumentOutOfRangeException(
            string.Format("On the property {0} {1} the Range attribute has an invalid range", property.PropertyType, property.Name));
      }

      return new RangeContstraints<T>((T) rangeAttribute.Minimum, (T) rangeAttribute.Maximum);
    }
  }
}