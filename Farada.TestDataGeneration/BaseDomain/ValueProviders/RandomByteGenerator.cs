using System;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random byte
  /// </summary>
  public class RandomByteGenerator:ValueProvider<byte>
  {
    protected override byte CreateValue (ValueProviderContext<byte> context)
    {
      var randomBytes = new byte[1];
      context.Random.NextBytes(randomBytes);

      return randomBytes[0];
    }
  }
}