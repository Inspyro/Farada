using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IRandomAndValueConfigurator:IRandomConfiguratorReturningValueConfigurator, IChainConfigurator
  {
  }
}