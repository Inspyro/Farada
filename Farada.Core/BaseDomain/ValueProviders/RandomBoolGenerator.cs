using System;
using Farada.Core.ValueProvider;

namespace Farada.Core.BaseDomain.ValueProviders
{
  internal class RandomBoolGenerator:ValueProvider<bool>
  {
    protected override bool CreateValue (ValueProviderContext<bool> context)
    {
      return context.Random.NextDouble() >= 0.5d;
    }
  }
}