using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProvider;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomShortGenerator:ValueProvider<short>
  {
    protected override short CreateValue (ValueProviderContext<short> context)
    {
      return context.Random.NextShort();
    }
  }
}