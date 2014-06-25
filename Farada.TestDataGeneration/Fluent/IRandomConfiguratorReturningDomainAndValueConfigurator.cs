using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IRandomConfiguratorReturningDomainAndValueConfigurator
  {
    IDomainAndValueConfigurator UseRandom (Random random);
  }
}