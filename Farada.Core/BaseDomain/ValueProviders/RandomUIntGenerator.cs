using System;
using Farada.Core.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

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