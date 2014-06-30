using System;
using System.ComponentModel.DataAnnotations;
using Farada.TestDataGeneration.FastReflection;

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
    /// Reads the <see cref="MinLengthAttribute"/> and <see cref="MaxLengthAttribute"/> as well as the <see cref="StringLengthAttribute"/> from an <see cref="IFastPropertyInfo"/> and creates a new RangeConstraints object with type from the <see cref="IFastPropertyInfo"/>
    /// 
    /// If a min length but no max length attribute is specified than max length is min length + 100
    /// If a max length but no min length attribute is specified then min length is max length - 100, but always > 0 
    /// </summary>
    /// <param name="property">The property for which the string constraints should be extracted</param>
    /// <returns>the resulting string constraints or null if no attribute was found</returns>
    /// <exception cref="ArgumentOutOfRangeException">throws when maxLength is smaller than minLength - so the attributes are declared in an incorrect manner</exception>
    public static StringConstraints FromProperty(IFastPropertyInfo property)
    {
      if (property == null)
        return null;

      var minLength = -1;
      var maxLength = -1;

      bool minLengthDefined = false;
      bool maxLengthDefined = false;

      if(property.IsDefined(typeof(MinLengthAttribute)))
      {
        var minLengthAttribute=property.GetCustomAttribute<MinLengthAttribute>();
        minLength = minLengthAttribute.Length;
        minLengthDefined = true;
      }

      if(property.IsDefined(typeof(MaxLengthAttribute)))
      {
        var maxLengthAttribute=property.GetCustomAttribute<MaxLengthAttribute>();
        maxLength = maxLengthAttribute.Length;
        maxLengthDefined = true;
      }

      if (minLengthDefined && maxLengthDefined && maxLength < minLength)
      {
        throw new ArgumentOutOfRangeException(
            string.Format("On the property {0} {1} the MinLength attribute and MaxLength attribute result in an invalid range", property.PropertyType, property.Name));
      }

      if (property.IsDefined(typeof (StringLengthAttribute)))
      {
        var stringLengthAttribute = property.GetCustomAttribute<StringLengthAttribute>();
        minLength = stringLengthAttribute.MinimumLength;
        maxLength = stringLengthAttribute.MaximumLength;

        if (maxLength < minLength)
        {
          throw new ArgumentOutOfRangeException(
              string.Format("On the property {0} {1} the StringLength attribute has an invalid range", property.PropertyType, property.Name));
        }
      }

      if (minLength < 0 && maxLength < 0)
        return null;

      if(minLengthDefined&&!maxLengthDefined)
      {
        maxLength = minLength + 100;
      }

      if (maxLengthDefined&&!minLengthDefined)
      {
        minLength = maxLength - 100;
      }

      if(minLength<0)
      {
        minLength = 0;
      }

      return new StringConstraints(minLength, maxLength);
    }
  }
}