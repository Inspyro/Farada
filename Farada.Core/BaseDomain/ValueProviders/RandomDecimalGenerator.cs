using System;
using Farada.Core.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

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