using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Modifiers;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IChainConfigurator
  {
    IValueProviderAndChainConfigurator<TProperty> For<TProperty>();
    IValueProviderAndChainConfigurator<TProperty> For<TContainer, TProperty> (Expression<Func<TContainer, TProperty>> memberExpression);
    IAttributeProviderAndChainConfigurator<TProperty, TAttribute> For<TProperty, TAttribute>() where TAttribute : Attribute;

    IChainConfigurator AddInstanceModifier (IInstanceModifier instanceModifier);
  }
}