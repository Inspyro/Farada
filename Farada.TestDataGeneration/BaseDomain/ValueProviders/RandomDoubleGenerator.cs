using System;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random double within the <see cref="RangeContstraints{T}"/> that are read from the property
  /// </summary>
  internal class RandomDoubleGenerator:ValueProvider<double, RangeConstrainedValueProviderContext<double>>
  {
    protected override RangeConstrainedValueProviderContext<double> CreateContext (ValueProviderObjectContext objectContext)
    {
      var rangeContstraints = RangeContstraints<double>.FromProperty(objectContext.PropertyInfo)
                              ?? new RangeContstraints<double>(double.MinValue, double.MaxValue);

      return new RangeConstrainedValueProviderContext<double>(objectContext, rangeContstraints);
    }

    protected override double CreateValue (RangeConstrainedValueProviderContext<double> context)
    {
      return context.Random.Next(context.RangeContstraints.MinValue, context.RangeContstraints.MaxValue);
    }
  }
}