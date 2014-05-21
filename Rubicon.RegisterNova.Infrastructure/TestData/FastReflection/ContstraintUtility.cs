using System;
using System.ComponentModel.DataAnnotations;

namespace Rubicon.RegisterNova.Infrastructure.TestData.FastReflection
{
  internal static class ContstraintUtility
  {
    internal static Constraints GetConstraints(IFastPropertyInfo property)
    {
      var constraints = new Constraints();

      if(property.IsDefined(typeof(MinLengthAttribute)))
      {
        var minLengthAttribute=property.GetCustomAttribute<MinLengthAttribute>();
        constraints.MinLength = minLengthAttribute.Length;
      }

      if(property.IsDefined(typeof(MaxLengthAttribute)))
      {
        var maxLengthAttribute=property.GetCustomAttribute<MaxLengthAttribute>();
        constraints.MaxLength = maxLengthAttribute.Length;
      }

      if(property.IsDefined(typeof(StringLengthAttribute)))
      {
        var stringLengthAttribute = property.GetCustomAttribute<StringLengthAttribute>();
        constraints.MinLength = stringLengthAttribute.MinimumLength;
        constraints.MaxLength = stringLengthAttribute.MaximumLength;
      }

      if(property.IsDefined(typeof(RangeAttribute)))
      {
        var rangeAttribute = property.GetCustomAttribute<RangeAttribute>();

        if(rangeAttribute.OperandType == typeof(int))
        {
          constraints.MinIntRange = (int) rangeAttribute.Minimum;
          constraints.MaxIntRange = (int) rangeAttribute.Maximum;
        }
        else if (rangeAttribute.OperandType == typeof (double))
        {
          constraints.MinDoubleRange = (double) rangeAttribute.Minimum;
          constraints.MaxDoubleRange = (double) rangeAttribute.Maximum;
        }
      }

      return constraints;
    }
  }
}
