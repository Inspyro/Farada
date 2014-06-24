using System;
using Farada.Core.ValueProvider;

namespace Farada.Core.BaseDomain.ValueProviders
{
  internal class RandomSByteGenerator:ValueProvider<sbyte>
  {
    protected override sbyte CreateValue (ValueProviderContext<sbyte> context)
    {
      return (sbyte) context.Random.Next(sbyte.MinValue, sbyte.MaxValue);
    }
  }
}