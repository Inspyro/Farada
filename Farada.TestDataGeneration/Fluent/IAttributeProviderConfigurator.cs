using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  public interface  IAttributeProviderConfigurator<TProperty, TAttribute>
      where TAttribute : Attribute
  {
    IAttributeProviderAndChainConfigurator<TProperty, TAttribute> AddProvider (AttributeBasedValueProvider<TProperty, TAttribute> attributeValueProvider);

    IAttributeProviderAndChainConfigurator<TProperty, TAttribute> AddProvider<TContext> (
        AttributeBasedValueProvider<TProperty, TAttribute, TContext> attributeValueProvider) where TContext : AttributeValueProviderContext<TProperty, TAttribute>;
  }
}