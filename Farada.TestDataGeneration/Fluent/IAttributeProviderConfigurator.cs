using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  public interface  IAttributeProviderConfigurator<TMember, TAttribute>
      where TAttribute : Attribute
  {
    IAttributeProviderAndChainConfigurator<TMember, TAttribute> AddProvider (AttributeBasedValueProvider<TMember, TAttribute> attributeValueProvider);
  }
}