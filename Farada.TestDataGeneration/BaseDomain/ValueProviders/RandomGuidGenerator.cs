using System;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomGuidGenerator : ValueProvider<Guid>
  {
    protected override Guid CreateValue (ValueProviderContext<Guid> context)
    {
      var guid = new byte[16];
      context.Random.NextBytes (guid);

      return new Guid (guid);
    }
  }
}