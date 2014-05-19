using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  internal class RandomBoolGenerator:ValueProvider<bool>
  {
    protected override bool CreateValue (ValueProviderContext<bool> context)
    {
      return context.Random.NextDouble() >= 0.5d;
    }
  }
}