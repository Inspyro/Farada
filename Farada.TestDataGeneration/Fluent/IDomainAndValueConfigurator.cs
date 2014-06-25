using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IDomainAndValueConfigurator:IDomainConfiguratorReturningValueConfigurator, IChainConfigurator
  {
  }
}