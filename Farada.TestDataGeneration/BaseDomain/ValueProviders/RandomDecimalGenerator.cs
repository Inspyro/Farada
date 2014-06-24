using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProvider;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomDecimalGenerator:ValueProvider<decimal>
  {
    protected override decimal CreateValue (ValueProviderContext<decimal> context)
    {
      return context.Random.Next(decimal.MinValue);
    }
  }
}