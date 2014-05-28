using System;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.Constraints;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  internal class RandomDoubleGenerator:ValueProvider<double, RangeConstrainedValueProviderContext<double>>
  {
    public override RangeConstrainedValueProviderContext<double> CreateContext (ValueProviderObjectContext objectContext)
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