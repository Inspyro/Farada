using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.Fluent;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration
{
  /// <summary>
  /// This class provides extension methods for the <see cref="IValueProviderAndChainConfigurator{TContainer,TMember}"/> and for the <see cref="IValueProviderConfigurator{TProperty}"/> in order to declare funcs instead
  /// of writing fully blown value providers
  /// <see cref="FuncProvider{T}"/> and <see cref="FuncProvider{TMember,TAttribute}"/>
  /// </summary>
  public static class IProviderConfiguratorExtensions
  {
    /// <summary>
    /// Adds a provider for a property in the chain
    /// </summary>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="configurator">The value configurator</param>
    /// <param name="reflectedType">the real type to create for the reflective value provider.</param>
    /// <param name="valueProviderFunc">the func generating the value</param>
    public static TReturn AddProvider<TReturn> (
        this IValueProviderConfigurator<TReturn, object> configurator,
        Type reflectedType,
        Func<ValueProviderContext<object>, object> valueProviderFunc)
    {
      return configurator.AddProvider (new ReflectiveFuncProvider (reflectedType, valueProviderFunc));
    }

    /// <summary>
    /// Adds a provider for a property in the chain
    /// </summary>
    /// <typeparam name="TMember">The type of the property</typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="configurator">The value configurator</param>
    /// <param name="valueProviderFunc">the func generating the value</param>
    public static TReturn AddProvider<TReturn, TMember> (
        this IValueProviderConfigurator<TReturn, TMember> configurator,
        Func<ValueProviderContext<TMember>, TMember> valueProviderFunc)
    {
      return configurator.AddProvider (new FuncProvider<TMember> (valueProviderFunc));
    }

  
    public static TReturn AddProvider<TReturn, TMember, TMetadata>(
        this IValueProviderWithMetadataConfigurator<TReturn, TMember, TMetadata> configurator,
        Func<MetatadaValueProviderContext<TMember, TMetadata>, TMember> valueProviderFunc)
    {
      return configurator.AddProvider(new FuncValueProviderWithMetadata<TMember, TMetadata>(valueProviderFunc));
    }

    //NOTE: ATM we do this just for the container - later also for the member as its easier..
    public static IContainerConfigurator<TMember> AddProvider<TMember, TAttribute> (
        this IValueProviderConfigurator<IContainerConfigurator<TMember>, TMember> configurator,
        Func<AttributeValueProviderContext<TMember, TAttribute>, TMember> valueProviderFunc)
        where TAttribute : Attribute
    {
      return configurator.AddProvider (new AttributeFuncProvider<TMember, TAttribute> (valueProviderFunc));
    }
  }
}