using System;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IAttributeProviderAndChainConfigurator<TMember, TAttribute>:IChainConfigurator, IAttributeProviderConfigurator<TMember, TAttribute>
      where TAttribute : Attribute
  {
  }
}