using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates random ulongs
  /// </summary>
  internal class RandomULongGenerator:ValueProvider<ulong>
  {
    protected override ulong CreateValue (ValueProviderContext<ulong> context)
    {
      return context.Random.Next(ulong.MinValue);
    }
  }
}