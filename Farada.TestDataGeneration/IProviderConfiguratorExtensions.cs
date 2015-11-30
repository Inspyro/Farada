using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.Fluent;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration
{
  /// <summary>
  /// This class provides extension methods for the <see cref="IValueProviderAndChainConfigurator{TMember}"/> and for the <see cref="IValueProviderConfigurator{TProperty}"/> in order to declare funcs instead
  /// of writing fully blown value providers
  /// <see cref="FuncProvider{T}"/> and <see cref="FuncProviderForAttributeBased{TMember,TAttribute}"/>
  /// </summary>
  public static class IProviderConfiguratorExtensions
  {
    /// <summary>
    /// Adds a provider for a property in the chain
    /// </summary>
    /// <typeparam name="TMember">The type of the property</typeparam>
    /// <param name="configurator">The value configurator</param>
    /// <param name="valueProviderFunc">the func generating the value</param>
    /// 
    public static IValueProviderAndChainConfigurator<TMember> AddProvider<TMember> (
        this IValueProviderConfigurator<TMember> configurator,
        Func<ValueProviderContext<TMember>, TMember> valueProviderFunc)
    {
      return configurator.AddProvider (new FuncProvider<TMember> (valueProviderFunc));
    }

    public static IValueProviderAndChainConfigurator<TMember> AddProvider<TMember, TAttribute> (
        this IValueProviderConfigurator<TMember> configurator,
        Func<ExtendedValueProviderContext<TMember, IList<TAttribute>>, TMember> valueProviderFunc)
        where TAttribute : Attribute
    {
      return configurator.AddProvider (new FuncProvider<TMember, TAttribute> (valueProviderFunc));
    }
  }
}