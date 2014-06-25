using System;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  internal class DefaultDomainRandomAndValueConfigurator : ChainConfigurator, IRandomAndValueConfigurator
  {
    internal DefaultDomainRandomAndValueConfigurator ()
        : base(new CompoundValueProviderBuilderFactory(new Random()).GetDefault())
    {
    }

    public IChainConfigurator UseRandom (Random random)
    {
      return new ChainConfigurator(new CompoundValueProviderBuilderFactory(random).GetDefault());
    }
  }
}