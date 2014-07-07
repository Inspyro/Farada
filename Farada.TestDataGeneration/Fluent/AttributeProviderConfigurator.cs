using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  internal class AttributeProviderConfigurator<TProperty, TAttribute> :ChainConfigurator, IAttributeProviderAndChainConfigurator<TProperty, TAttribute>
      where TAttribute : Attribute
  {
    internal AttributeProviderConfigurator (Func<CompoundValueProviderBuilder> lazyValueProviderBuilder)
        : base(lazyValueProviderBuilder)
    {
    }

    public IAttributeProviderAndChainConfigurator<TProperty, TAttribute> AddProvider (AttributeBasedValueProvider<TProperty, TAttribute> attributeValueProvider)
    {
      _lazyValueProviderBuilder().AddProvider(attributeValueProvider);
      return this;
    }

    public IAttributeProviderAndChainConfigurator<TProperty, TAttribute> AddProvider<TContext> (AttributeBasedValueProvider<TProperty, TAttribute, TContext> attributeValueProvider) where TContext : AttributeValueProviderContext<TProperty, TAttribute>
    {
      _lazyValueProviderBuilder().AddProvider(attributeValueProvider);
      return this;
    }
  }
}