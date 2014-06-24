using System;
using Farada.TestDataGeneration.ValueProvider;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomSByteGenerator:ValueProvider<sbyte>
  {
    protected override sbyte CreateValue (ValueProviderContext<sbyte> context)
    {
      return (sbyte) context.Random.Next(sbyte.MinValue, sbyte.MaxValue);
    }
  }
}