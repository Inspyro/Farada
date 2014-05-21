using System;
using System.Linq.Expressions;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  public static class CompoundValueProviderBuilderExtensions
  {
    public static void AddProvider<TProperty> (
        this ICompoundValueProviderBuilder builder,
        Func<ValueProviderContext<TProperty>, TProperty> valueProviderFunc)
    {
      builder.AddProvider(new FuncProvider<TProperty>(valueProviderFunc));
    }

    public static void AddProvider<TProperty, TAttribute> (this ICompoundValueProviderBuilder builder, Func<AttributeValueProviderContext<TProperty, TAttribute>, TProperty> attributeValueProviderFunc) where TAttribute : Attribute
    {
      builder.AddProvider(new FuncProviderForAttribute<TProperty, TAttribute>(attributeValueProviderFunc));
    }

    public static void AddProvider<TProperty, TContainer> (this ICompoundValueProviderBuilder builder, Expression<Func<TContainer, TProperty>> chainExpression, Func<ValueProviderContext<TProperty>, TProperty> valueProviderFunc)
    {
      builder.AddProvider(chainExpression, new FuncProvider<TProperty>(valueProviderFunc));
    }
  }
}