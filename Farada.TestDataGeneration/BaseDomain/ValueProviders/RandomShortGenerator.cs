using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random short
  /// </summary>
  public class RandomShortGenerator:ValueProvider<short>
  {
    protected override short CreateValue (ValueProviderContext<short> context)
    {
      return context.Random.NextShort();
    }
  }
}