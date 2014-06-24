using System;
using Farada.Core.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Farada.Core.BaseDomain.ValueProviders
{
  internal class RandomUShortGenerator:ValueProvider<ushort>
  {
    protected override ushort CreateValue (ValueProviderContext<ushort> context)
    {
      return context.Random.NextUShort();
    }
  }
}