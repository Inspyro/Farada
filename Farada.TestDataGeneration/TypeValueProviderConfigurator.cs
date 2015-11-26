using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Fluent;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration
{
  internal class TypeValueProviderConfigurator<TType>:ChainConfigurator, IValueProviderAndChainConfigurator<TType>
  {
    internal TypeValueProviderConfigurator (Func<CompoundValueProviderBuilder> lazyValueProviderBuilder)
        : base(lazyValueProviderBuilder)
    {
    }

    public IValueProviderAndChainConfigurator<TType> AddProvider (ValueProvider<TType> valueProvider)
    {
      _lazyValueProviderBuilder().AddProvider(valueProvider);
      return this;
    }

    public IValueProviderAndChainConfigurator<TType> AddProvider<TContext> (ValueProvider<TType, TContext> valueProvider) where TContext : ValueProviderContext<TType>
    {
      _lazyValueProviderBuilder().AddProvider(valueProvider);
      return this;
    }

    public IValueProviderAndChainConfigurator<TType> DisableAutoFill ()
    {
      _lazyValueProviderBuilder().DisableAutoFill<TType>();
      return this;
    }
  }
}