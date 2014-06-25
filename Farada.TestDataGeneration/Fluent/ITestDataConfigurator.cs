using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface ITestDataConfigurator:IChainConfigurator, IDomainConfiguratorReturningRandomAndValueConfigurator, IRandomConfiguratorReturningDomainAndValueConfigurator
  {

  }
}