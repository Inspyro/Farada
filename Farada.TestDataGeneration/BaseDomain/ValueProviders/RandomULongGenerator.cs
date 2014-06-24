using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProvider;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomULongGenerator:ValueProvider<ulong>
  {
    protected override ulong CreateValue (ValueProviderContext<ulong> context)
    {
      return context.Random.Next(ulong.MinValue);
    }
  }
}