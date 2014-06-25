using System;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  internal class DomainConfigurator
      : ChainConfigurator, ITestDataConfigurator

  {
    protected internal DomainConfigurator ()
        : base(new CompoundValueProviderBuilderFactory(new Random()).GetEmpty())
    {
    }

    public IRandomAndValueConfigurator UseDefaults (bool useDefaults)
    {
      return useDefaults ? (IRandomAndValueConfigurator) new DefaultDomainRandomAndValueConfigurator() : new EmptyDomainRandomAndValueConfigurator();
    }

    public IDomainAndValueConfigurator UseRandom (Random random)
    {
      return new DomainAndValueConfigurator(new CompoundValueProviderBuilderFactory(random));
    }
  }
}