using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  internal class RandomByteGenerator:ValueProvider<byte>
  {
    protected override byte CreateValue (ValueProviderContext<byte> context)
    {
      var randomBytes = new byte[1];
      context.Random.NextBytes(randomBytes);

      return randomBytes[0];
    }
  }
}