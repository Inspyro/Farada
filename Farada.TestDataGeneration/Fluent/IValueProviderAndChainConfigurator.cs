using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IValueProviderAndChainConfigurator<TProperty> : IChainConfigurator, IValueProviderConfigurator<TProperty>
  {
  }
}