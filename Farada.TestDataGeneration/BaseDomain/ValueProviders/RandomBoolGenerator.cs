using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random boolean
  /// </summary>
  internal class RandomBoolGenerator:ValueProvider<bool>
  {
    protected override bool CreateValue (ValueProviderContext<bool> context)
    {
      return context.Random.NextDouble() >= 0.5d;
    }
  }
}