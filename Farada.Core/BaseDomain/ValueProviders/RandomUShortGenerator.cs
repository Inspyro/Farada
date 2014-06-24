using System;
using Farada.Core.Extensions;
using Farada.Core.ValueProvider;

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