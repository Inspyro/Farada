using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random long
  /// </summary>
  internal class RandomLongGenerator:ValueProvider<long> //TODO: use range constraints?
  {
    protected override long CreateValue (ValueProviderContext<long> context)
    {
      return context.Random.Next(long.MinValue);
    }
  }
}