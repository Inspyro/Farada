using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random decimal
  /// </summary>
  public class RandomDecimalGenerator:ValueProvider<decimal>
  {
    protected override decimal CreateValue (ValueProviderContext<decimal> context)
    {
      return context.Random.Next(decimal.MinValue);
    }
  }
}