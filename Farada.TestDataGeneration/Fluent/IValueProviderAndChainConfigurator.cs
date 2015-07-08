using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IValueProviderAndChainConfigurator<TMember> : IChainConfigurator, IValueProviderConfigurator<TMember>
  {
  }
}