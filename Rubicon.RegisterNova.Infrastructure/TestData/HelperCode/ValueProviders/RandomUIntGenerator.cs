using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  internal class RandomUIntGenerator:ValueProvider<uint>
  {
    protected override uint CreateValue (ValueProviderContext<uint> context)
    {
      return context.Random.Next(uint.MinValue);
    }
  }
}