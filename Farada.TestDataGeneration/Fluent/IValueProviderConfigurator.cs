using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IValueProviderConfigurator<TProperty>
  {
    IValueProviderAndChainConfigurator<TProperty> AddProvider (ValueProvider<TProperty> valueProvider);
    IValueProviderAndChainConfigurator<TProperty> AddProvider<TContext> (ValueProvider<TProperty, TContext> valueProvider) where TContext : ValueProviderContext<TProperty>;
  }
}