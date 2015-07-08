using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IValueProviderConfigurator<TMember>
  {
    IValueProviderAndChainConfigurator<TMember> AddProvider (ValueProvider<TMember> valueProvider);
    IValueProviderAndChainConfigurator<TMember> AddProvider<TContext> (ValueProvider<TMember, TContext> valueProvider) where TContext : ValueProviderContext<TMember>;
  }
}