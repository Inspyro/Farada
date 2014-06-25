using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random short
  /// </summary>
  internal class RandomShortGenerator:ValueProvider<short>
  {
    protected override short CreateValue (ValueProviderContext<short> context)
    {
      return context.Random.NextShort();
    }
  }
}