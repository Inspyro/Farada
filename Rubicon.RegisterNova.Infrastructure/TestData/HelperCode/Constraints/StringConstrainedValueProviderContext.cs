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
      }

      if (minLength < 0 && maxLength < 0)
        return null;

      return new StringConstraints(minLength, maxLength);
    }
  }
}