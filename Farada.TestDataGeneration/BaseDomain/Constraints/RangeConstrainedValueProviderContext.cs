using System;
using System.ComponentModel.DataAnnotations;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.ValueProvider;

namespace Farada.TestDataGeneration.BaseDomain.Constraints
{
  public class RangeConstrainedValueProviderContext<T>:ValueProviderContext<T>
      where T : IComparable
  {
    public RangeContstraints<T> RangeContstraints { get; private set; }

    internal RangeConstrainedValueProviderContext (ValueProviderObjectContext objectContext, RangeContstraints<T> rangeContstraints)
        : base(objectContext)
    {
      RangeContstraints = rangeContstraints;
    }
  }

   public class RangeContstraints<T> where T:IComparable
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

      if (((T) rangeAttribute.Minimum).CompareTo((T) rangeAttribute.Maximum) > 0)
      {
        throw new ArgumentOutOfRangeException(
            string.Format("On the property {0} {1} the Range attribute has an invalid range", property.PropertyType, property.Name));
      }

      return new RangeContstraints<T>((T) rangeAttribute.Minimum, (T) rangeAttribute.Maximum);
    }
  }
}