using System;
using Farada.Core.Extensions;
using Farada.Core.ValueProvider;

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