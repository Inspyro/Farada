using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  internal class RandomUShortGenerator:ValueProvider<ushort>
  {
    protected override ushort CreateValue (ValueProviderContext<ushort> context)
    {
      return context.Random.NextUShort();
    }
  }
}