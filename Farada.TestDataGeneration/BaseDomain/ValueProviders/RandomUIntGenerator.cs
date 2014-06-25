using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomUIntGenerator:ValueProvider<uint>
  {
    protected override uint CreateValue (ValueProviderContext<uint> context)
    {
      return context.Random.Next(uint.MinValue);
    }
  }
}