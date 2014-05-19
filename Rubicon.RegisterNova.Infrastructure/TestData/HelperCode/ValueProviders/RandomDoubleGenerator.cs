using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  internal class RandomDoubleGenerator:ValueProvider<double>
  {
    protected override double CreateValue (ValueProviderContext<double> context)
    {
      return context.Random.Next(double.MinValue);
    }
  }
}