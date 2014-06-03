using System;
using System.ComponentModel.DataAnnotations;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.Constraints
{
  public class StringConstrainedValueProviderContext:ValueProviderContext<string>
  {
    public StringConstraints StringConstraints { get; private set; }

    internal StringConstrainedValueProviderContext (ValueProviderObjectContext objectContext, StringConstraints stringConstraints)
        : base(objectContext)
    {
      StringConstraints = stringConstraints;
    }
  }

  public class StringConstraints
  {
    public int MinLength { get; private set; }
    public int MaxLength { get; private set; }

    public StringConstraints (int minLength, int maxLength)
    {
      MinLength = minLength;
      MaxLength = maxLength;
    }

    public static StringConstraints FromProperty(IFastPropertyInfo property)
    {
      if (property == null)
        return null;

      var minLength = -1;
      var maxLength = -1; 
      if(property.IsDefined(typeof(MinLengthAttribute)))
      {
        var minLengthAttribute=property.GetCustomAttribute<MinLengthAttribute>();
        minLength = minLengthAttribute.Length;
      }

      if(property.IsDefined(typeof(MaxLengthAttribute)))
      {
        var maxLengthAttribute=property.GetCustomAttribute<MaxLengthAttribute>();
        maxLength = maxLengthAttribute.Length;
      }

      if (property.IsDefined(typeof (StringLengthAttribute)))
      {
        var stringLengthAttribute = property.GetCustomAttribute<StringLengthAttribute>();
        minLength = stringLengthAttribute.MinimumLength;
        maxLength = stringLengthAttribute.MaximumLength;

        if(maxLength<minLength)
        {
          throw new ArgumentOutOfRangeException(
              string.Format("On the property {0} {1} the StringLength attribute has an invalid range", property.PropertyType, property.Name));
        }
      }

      if (minLength < 0 && maxLength < 0)
        return null;

      if(maxLength<minLength)
      {
        maxLength = minLength + 100;
      }

      if (minLength > maxLength)
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