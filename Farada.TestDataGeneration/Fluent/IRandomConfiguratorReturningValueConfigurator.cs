using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IRandomConfiguratorReturningValueConfigurator
  {
    IChainConfigurator UseRandom (Random random);
  }
}