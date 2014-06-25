using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IDomainConfiguratorReturningRandomAndValueConfigurator
  {
    IRandomAndValueConfigurator UseDefaults (bool useDefaults);
  }
}