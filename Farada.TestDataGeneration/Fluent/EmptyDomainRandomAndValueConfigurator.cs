using System;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  internal class EmptyDomainRandomAndValueConfigurator : ChainConfigurator, IRandomAndValueConfigurator
  {
    internal EmptyDomainRandomAndValueConfigurator ()
        : base(new CompoundValueProviderBuilderFactory(new Random()).GetEmpty())
    {
    }

    public IChainConfigurator UseRandom (Random random)
    {
      return new ChainConfigurator(new CompoundValueProviderBuilderFactory(random).GetEmpty());
    }
  }
}