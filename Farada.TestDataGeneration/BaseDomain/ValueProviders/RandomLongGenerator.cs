using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomLongGenerator:ValueProvider<long>
  {
    protected override long CreateValue (ValueProviderContext<long> context)
    {
      return context.Random.Next(long.MinValue);
    }
  }
}