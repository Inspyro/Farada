using System;
using Farada.Core.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Farada.Core.BaseDomain.ValueProviders
{
  internal class RandomShortGenerator:ValueProvider<short>
  {
    protected override short CreateValue (ValueProviderContext<short> context)
    {
      return context.Random.NextShort();
    }
  }
}