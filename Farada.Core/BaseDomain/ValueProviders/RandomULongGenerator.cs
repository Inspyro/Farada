using System;
using Farada.Core.Extensions;
using Farada.Core.ValueProvider;

namespace Farada.Core.BaseDomain.ValueProviders
{
  internal class RandomULongGenerator:ValueProvider<ulong>
  {
    protected override ulong CreateValue (ValueProviderContext<ulong> context)
    {
      return context.Random.Next(ulong.MinValue);
    }
  }
}