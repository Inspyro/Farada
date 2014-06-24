using System;
using Farada.Core.Extensions;
using Farada.Core.ValueProvider;

namespace Farada.Core.BaseDomain.ValueProviders
{
  internal class RandomDecimalGenerator:ValueProvider<decimal>
  {
    protected override decimal CreateValue (ValueProviderContext<decimal> context)
    {
      return context.Random.Next(decimal.MinValue);
    }
  }
}