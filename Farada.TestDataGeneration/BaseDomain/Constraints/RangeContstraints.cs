using System;
using System.ComponentModel.DataAnnotations;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.BaseDomain.Constraints
{
  /// <summary>
  /// Defines range constraints for a type, that is usually numeric
  /// </summary>
  /// <typeparam name="T">the type of the constraints</typeparam>
  public class RangeContstraints<T>
      where T : IComparable
  {
    /// <summary>
    /// The minimum value that is allowed
    /// </summary>
    public T MinValue { get; private set; }

    /// <summary>
    /// The maximum value that is allowed
    /// </summary>
    public T MaxValue { get; private set; }

    public RangeContstraints (T minValue, T maxValue)
    {
      MinValue = minValue;
      MaxValue = maxValue;
    }

    /// <summary>
    /// Reads the <see cref="RangeAttribute"/> from an <see cref="IFastMemberWithValues"/> and creates a new RangeConstraints object with type from the <see cref="IFastMemberWithValues"/>
    /// </summary>
    /// <param name="member">The member for which the range constraints should be extracted</param>
    /// <returns></returns>
    [CanBeNull]
    public static RangeContstraints<T> FromMember ([CanBeNull] IFastMemberWithValues member)
    {
      if (member == null)
        return null;

      if (!member.IsDefined (typeof (RangeAttribute)))
        return null;

      var rangeAttribute = member.GetCustomAttribute<RangeAttribute>();

      if (rangeAttribute == null)
        return null;

      if (rangeAttribute.OperandType != typeof (T))
        return null;

      var minValue = (T) Convert.ChangeType (rangeAttribute.Minimum, typeof (T));
      var maxValue = (T) Convert.ChangeType (rangeAttribute.Maximum, typeof (T));

      if ((minValue).CompareTo (maxValue) > 0)
      {
        throw new ArgumentOutOfRangeException (
            string.Format ("On the member {0} {1} the Range attribute has an invalid range", member.Type, member.Name));
      }

      return new RangeContstraints<T> (minValue, maxValue);
    }
  }
}