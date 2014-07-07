using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Fluent;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration
{
  internal class TypeValueProviderConfigurator<TProperty>:ChainConfigurator, IValueProviderAndChainConfigurator<TProperty>
  {
    internal TypeValueProviderConfigurator (Func<CompoundValueProviderBuilder> lazyValueProviderBuilder)
        : base(lazyValueProviderBuilder)
    {
    }

    public IValueProviderAndChainConfigurator<TProperty> AddProvider (ValueProvider<TProperty> valueProvider)
    {
      _lazyValueProviderBuilder().AddProvider(valueProvider);
      return this;
    }

    public IValueProviderAndChainConfigurator<TProperty> AddProvider<TContext> (ValueProvider<TProperty, TContext> valueProvider) where TContext : ValueProviderContext<TProperty>
    {
      _lazyValueProviderBuilder().AddProvider(valueProvider);
      return this;
    }
  }
}