using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IAttributeProviderAndChainConfigurator<TProperty, TAttribute>:IChainConfigurator, IAttributeProviderConfigurator<TProperty, TAttribute>
      where TAttribute : Attribute
  {
  }
}