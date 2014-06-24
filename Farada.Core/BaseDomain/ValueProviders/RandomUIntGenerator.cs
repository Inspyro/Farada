using System;
using Farada.Core.Extensions;
using Farada.Core.ValueProvider;

namespace Farada.Core.BaseDomain.ValueProviders
{
  internal class RandomUIntGenerator:ValueProvider<uint>
  {
    protected override uint CreateValue (ValueProviderContext<uint> context)
    {
      return context.Random.Next(uint.MinValue);
    }
  }
}