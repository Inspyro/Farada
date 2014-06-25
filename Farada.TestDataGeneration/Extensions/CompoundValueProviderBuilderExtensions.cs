using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Extensions
{
  /// <summary>
  /// This class provides extension methods for the <see cref="CompoundValueProviderBuilder"/> in order to declare funcs instead
  /// of writing fully blown value providers
  /// <see cref="FuncProvider{T}"/> and <see cref="FuncProviderForAttributeBased{TProperty,TAttribute}"/>
  /// </summary>
  public static class CompoundValueProviderBuilderExtensions
  {
    /// <summary>
    /// Adds a provider for an attribute and a given return type
    /// You need to inject a provider for each attribute/type pair that you want to be filled
    /// </summary>
    /// <typeparam name="TProperty">The type of the properties which should filled</typeparam>
    /// <typeparam name="TAttribute">The type of the attribute that should be on the properties</typeparam>
    /// <typeparam name="TContainer">The type of the containing class</typeparam>
    /// <param name="builder">The builder to add the func provider to</param>
    /// <param name="chainExpression">The expression to define the property and attribute type</param>
    /// <param name="attributeValueProviderFunc">the func generating the value</param>
    public static void AddProvider<TProperty, TAttribute, TContainer> (this ICompoundValueProviderBuilder builder,  Expression<Func<TContainer, TAttribute, TProperty>> chainExpression, Func<AttributeValueProviderContext<TProperty, TAttribute>, TProperty> attributeValueProviderFunc) where TAttribute : Attribute
    {
      builder.AddProvider(chainExpression, new FuncProviderForAttributeBased<TProperty, TAttribute>(attributeValueProviderFunc));
    }

    /// <summary>
    /// Adds a provider for a property in the chain
    /// </summary>
    /// <typeparam name="TProperty">The type of the property</typeparam>
    /// <typeparam name="TContainer">The type containing the property or the type itself</typeparam>
    /// <param name="builder">The builder to add the func provider to</param>
    /// <param name="chainExpression">The expression that leads to the property (e.g. (Person p)=>p.Name) or to the type (e.g. (string s)=>s)</param>
    /// <param name="valueProviderFunc">the func generating the value</param>
    public static void AddProvider<TProperty, TContainer> (this ICompoundValueProviderBuilder builder, Expression<Func<TContainer, TProperty>> chainExpression, Func<ValueProviderContext<TProperty>, TProperty> valueProviderFunc)
    {
      builder.AddProvider(chainExpression, new FuncProvider<TProperty>(valueProviderFunc));
    }
  }
}