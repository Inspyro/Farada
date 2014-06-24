using System;
using Farada.Core.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Farada.Core.BaseDomain.ValueProviders
{
  internal class RandomLongGenerator:ValueProvider<long>
  {
    protected override long CreateValue (ValueProviderContext<long> context)
    {
      return context.Random.Next(long.MinValue);
    }
  }
}