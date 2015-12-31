using System;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random sbyte
  /// </summary>
  public class RandomSByteGenerator:ValueProvider<sbyte>
  {
    protected override sbyte CreateValue (ValueProviderContext<sbyte> context)
    {
      return (sbyte) context.Random.Next(sbyte.MinValue, sbyte.MaxValue);
    }
  }
}