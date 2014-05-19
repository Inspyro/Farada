using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  internal class RandomULongGenerator:ValueProvider<ulong>
  {
    protected override ulong CreateValue (ValueProviderContext<ulong> context)
    {
      return context.Random.Next(ulong.MinValue);
    }
  }
}