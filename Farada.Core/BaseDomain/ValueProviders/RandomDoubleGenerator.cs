using System;
using Farada.Core.BaseDomain.Constraints;
using Farada.Core.Extensions;
using Farada.Core.ValueProvider;

namespace Farada.Core.BaseDomain.ValueProviders
{
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