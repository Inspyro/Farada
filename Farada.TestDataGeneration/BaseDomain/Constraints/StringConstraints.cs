using System;
using System.ComponentModel.DataAnnotations;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.BaseDomain.Constraints
{
  /// <summary>
  /// Defines constraints for a string
  /// </summary>
  public class StringConstraints
  {
    /// <summary>
    /// The minimum character length of the string
    /// </summary>
    public int MinLength { get; private set; }

    /// <summary>
    /// The maximum character length fo the string
    /// </summary>
    public int MaxLength { get; private set; }

    public StringConstraints (int minLength, int maxLength)
    {
      MinLength = minLength;
      MaxLength = maxLength;
    }

    /// <summary>
    /// Reads the <see cref="MinLengthAttribute"/> and <see cref="MaxLengthAttribute"/> as well as the <see cref="StringLengthAttribute"/> from an <see cref="IFastMemberWithValues"/> and creates a new RangeConstraints object with type from the <see cref="IFastMemberWithValues"/>
    /// 
    /// If a min length but no max length attribute is specified than max length is min length + 100
    /// If a max length but no min length attribute is specified then min length is max length - 100, but always > 0 
    /// </summary>
    /// <param name="member">The member for which the string constraints should be extracted</param>
    /// <returns>the resulting string constraints or null if no attribute was found</returns>
    /// <exception cref="ArgumentOutOfRangeException">throws when maxLength is smaller than minLength - so the attributes are declared in an incorrect manner</exception>
    public static StringConstraints FromMember ([CanBeNull] IFastMemberWithValues member)
    {
      if (member == null)
        return null;

      var minLength = -1;
      var maxLength = -1;

      var minLengthDefined = false;
      var maxLengthDefined = false;

      if (member.IsDefined (typeof (MinLengthAttribute)))
      {
        var minLengthAttribute = member.GetCustomAttribute<MinLengthAttribute>();
        minLength = minLengthAttribute.Length;
        minLengthDefined = true;
      }

      if (member.IsDefined (typeof (MaxLengthAttribute)))
      {
        var maxLengthAttribute = member.GetCustomAttribute<MaxLengthAttribute>();
        maxLength = maxLengthAttribute.Length;
        maxLengthDefined = true;
      }

      if (minLengthDefined && maxLengthDefined && maxLength < minLength)
      {
        throw new ArgumentOutOfRangeException (
            string.Format (
                "On the member {0} {1} the MinLength attribute and MaxLength attribute result in an invalid range",
                member.Type,
                member.Name));
      }

      if (member.IsDefined (typeof (StringLengthAttribute)))
      {
        var stringLengthAttribute = member.GetCustomAttribute<StringLengthAttribute>();
        minLength = stringLengthAttribute.MinimumLength;
        maxLength = stringLengthAttribute.MaximumLength;

        if (maxLength < minLength)
        {
          throw new ArgumentOutOfRangeException (
              string.Format ("On the member {0} {1} the StringLength attribute has an invalid range", member.Type, member.Name));
        }
      }

      if (minLength < 0 && maxLength < 0)
        return null;

      if (minLengthDefined && !maxLengthDefined)
      {
        maxLength = minLength + 100;
      }

      if (maxLengthDefined && !minLengthDefined)
      {
        minLength = maxLength - 100;
      }

      if (minLength < 0)
      {
        minLength = 0;
      }

      return new StringConstraints (minLength, maxLength);
    }
  }
}