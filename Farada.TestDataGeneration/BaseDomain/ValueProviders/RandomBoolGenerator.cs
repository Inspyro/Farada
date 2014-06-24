using System;
using Farada.TestDataGeneration.ValueProvider;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomBoolGenerator:ValueProvider<bool>
  {
    protected override bool CreateValue (ValueProviderContext<bool> context)
    {
      return context.Random.NextDouble() >= 0.5d;
    }
  }
}