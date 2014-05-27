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
    public static void AddProvider<TProperty, TAttribute, TContainer> (this ICompoundValueProviderBuilder builder,  Expression<Func<TContainer, TAttribute, TProperty>> chainExpression, Func<AttributeValueProviderContext<TProperty, TAttribute>, TProperty> attributeValueProviderFunc) where TAttribute : Attribute
    {
      builder.AddProvider(chainExpression, new FuncProviderForAttribute<TProperty, TAttribute>(attributeValueProviderFunc));
    }

    public static void AddProvider<TProperty, TContainer> (this ICompoundValueProviderBuilder builder, Expression<Func<TContainer, TProperty>> chainExpression, Func<ValueProviderContext<TProperty>, TProperty> valueProviderFunc)
    {
      builder.AddProvider(chainExpression, new FuncProvider<TProperty>(valueProviderFunc));
    }
  }
}