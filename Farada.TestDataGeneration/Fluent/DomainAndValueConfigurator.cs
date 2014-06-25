using System;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  internal class DomainAndValueConfigurator : ChainConfigurator, IDomainAndValueConfigurator
  {
    private readonly CompoundValueProviderBuilderFactory _valueProviderBuilderFactory;

    internal DomainAndValueConfigurator (CompoundValueProviderBuilderFactory valueProviderBuilderFactory)
        : base(valueProviderBuilderFactory.GetEmpty())
    {
      _valueProviderBuilderFactory = valueProviderBuilderFactory;
    }

    public IChainConfigurator UseDefaults ()
    {
      return new ChainConfigurator(_valueProviderBuilderFactory.GetDefault());
    }
  }
}