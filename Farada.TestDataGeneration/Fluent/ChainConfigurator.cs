using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  internal class ChainConfigurator : IChainConfigurator
  {
    protected Func<CompoundValueProviderBuilder> _lazyValueProviderBuilder;
    internal ChainConfigurator(Func<CompoundValueProviderBuilder> lazyValueProviderBuilder)
    {
      _lazyValueProviderBuilder = lazyValueProviderBuilder;
    }

    public IValueProviderAndChainConfigurator<TType> For<TType> ()
    {
      return new TypeValueProviderConfigurator<TType>(_lazyValueProviderBuilder);
    }

    public IValueProviderAndChainConfigurator<TProperty> For<TContainer, TProperty> (Expression<Func<TContainer, TProperty>> memberExpression)
    {
      return new ExpressionValueProviderConfigurator<TContainer, TProperty>(memberExpression, _lazyValueProviderBuilder);
    }

    public IAttributeProviderAndChainConfigurator<TProperty, TAttribute> For<TProperty, TAttribute> () where TAttribute : Attribute
    {
      return new AttributeProviderConfigurator<TProperty, TAttribute>(_lazyValueProviderBuilder);
    }

    internal ITestDataGenerator Build ()
    {
      return _lazyValueProviderBuilder().Build();
    }
  }
}