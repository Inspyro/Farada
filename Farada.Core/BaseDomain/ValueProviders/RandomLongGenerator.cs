using System;
using Farada.Core.Extensions;
using Farada.Core.ValueProvider;

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