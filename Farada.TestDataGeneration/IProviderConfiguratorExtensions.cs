using System;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Fluent;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration
{
  /// <summary>
  /// This class provides extension methods for the <see cref="IAttributeProviderConfigurator{TMember,TAttribute}"/> and for the <see cref="IValueProviderConfigurator{TProperty}"/> in order to declare funcs instead
  /// of writing fully blown value providers
  /// <see cref="FuncProvider{T}"/> and <see cref="FuncProviderForAttributeBased{TMember,TAttribute}"/>
  /// </summary>
  public static class IProviderConfiguratorExtensions
  {
    /// <summary>
    /// Adds a provider for an attribute and a given return type
    /// You need to inject a provider for each attribute/type pair that you want to be filled
    /// </summary>
    /// <typeparam name="TMember">The type of the properties which should filled</typeparam>
    /// <typeparam name="TAttribute">The type of the attribute that should be on the properties</typeparam>
    /// <param name="attributeProviderConfigurator">The attribute provider configurator</param>
    /// <param name="attributeValueProviderFunc">the func generating the value</param>
    public static IAttributeProviderAndChainConfigurator<TMember, TAttribute> AddProvider<TMember, TAttribute> (this IAttributeProviderConfigurator<TMember, TAttribute> attributeProviderConfigurator, Func<AttributeValueProviderContext<TMember, TAttribute>, TMember> attributeValueProviderFunc) where TAttribute : Attribute
    {
     return attributeProviderConfigurator.AddProvider(new FuncProviderForAttributeBased<TMember, TAttribute>(attributeValueProviderFunc));
    }

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
     return configurator.AddProvider(new FuncProvider<TMember>(valueProviderFunc));
    }
  }
}