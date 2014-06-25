using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IDomainConfiguratorReturningValueConfigurator
  {
    IChainConfigurator UseDefaults ();
  }
}