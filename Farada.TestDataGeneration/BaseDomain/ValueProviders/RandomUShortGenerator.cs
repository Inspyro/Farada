using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomUShortGenerator:ValueProvider<ushort>
  {
    protected override ushort CreateValue (ValueProviderContext<ushort> context)
    {
      return context.Random.NextUShort();
    }
  }
}