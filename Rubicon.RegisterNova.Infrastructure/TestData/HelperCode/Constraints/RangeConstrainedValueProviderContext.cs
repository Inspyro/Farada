using System;
using System.ComponentModel.DataAnnotations;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.Constraints
{
  public class RangeConstrainedValueProviderContext<T>:ValueProviderContext<T>
  {
    public RangeContstraints<T> RangeContstraints { get; private set; }

    internal RangeConstrainedValueProviderContext (ValueProviderObjectContext objectContext, RangeContstraints<T> rangeContstraints)
        : base(objectContext)
    {
      RangeContstraints = rangeContstraints;
    }
  }

   public class RangeContstraints<T>
  {
    public T MinValue { get; private set; }
    public T MaxValue { get; private set; }

    public RangeContstraints(T minValue, T maxValue)
    {
      MinValue = minValue;
      MaxValue = maxValue;
    }

    public static RangeContstraints<T> FromProperty (IFastPropertyInfo property)
    {
      if (property == null || !property.IsDefined(typeof (RangeAttribute)))
        return null;

      var rangeAttribute = property.GetCustomAttribute<RangeAttribute>();

      if (rangeAttribute.OperandType != typeof (T))
        return null;

      return new RangeContstraints<T>((T) rangeAttribute.Minimum, (T) rangeAttribute.Maximum);
    }
  }
}