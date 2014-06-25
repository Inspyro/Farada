using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Modifiers;

namespace Farada.TestDataGeneration.Fluent
{
  internal class ChainConfigurator : IChainConfigurator
  {
    protected readonly CompoundValueProviderBuilder _valueProviderBuilder;
    internal ChainConfigurator(CompoundValueProviderBuilder valueProviderBuilder)
    {
      _valueProviderBuilder = valueProviderBuilder;
    }

    public IValueProviderAndChainConfigurator<TProperty> For<TProperty> ()
    {
      return new TypeValueProviderConfigurator<TProperty>(_valueProviderBuilder);
    }

    public IValueProviderAndChainConfigurator<TProperty> For<TContainer, TProperty> (Expression<Func<TContainer, TProperty>> memberExpression)
    {
      return new ExpressionValueProviderConfigurator<TContainer, TProperty>(memberExpression, _valueProviderBuilder);
    }

    public IAttributeProviderAndChainConfigurator<TProperty, TAttribute> For<TProperty, TAttribute> () where TAttribute : Attribute
    {
      return new AttributeProviderConfigurator<TProperty, TAttribute>(_valueProviderBuilder);
    }

    public IChainConfigurator AddInstanceModifier (IInstanceModifier instanceModifier)
    {
      _valueProviderBuilder.AddInstanceModifier(instanceModifier);
      return this;
    }

    internal ITestDataGenerator Build ()
    {
      return _valueProviderBuilder.Build();
    }
  }
}