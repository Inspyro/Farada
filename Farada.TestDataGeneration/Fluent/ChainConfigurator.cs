using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.Fluent
{
  internal class ChainConfigurator : IChainConfigurator
  {
    protected Func<CompoundValueProviderBuilder> _lazyValueProviderBuilder;
    internal ChainConfigurator([CanBeNull] Func<CompoundValueProviderBuilder> lazyValueProviderBuilder)
    {
      _lazyValueProviderBuilder = lazyValueProviderBuilder;
    }

    public IValueProviderAndChainConfigurator<TType> For<TType> ()
    {
      return new TypeValueProviderConfigurator<TType>(_lazyValueProviderBuilder);
    }

    public IValueProviderAndChainConfigurator<TMember> For<TContainer, TMember> (Expression<Func<TContainer, TMember>> memberExpression)
    {
      return new ExpressionValueProviderConfigurator<TContainer, TMember>(memberExpression, _lazyValueProviderBuilder);
    }

    internal ITestDataGenerator Build ()
    {
      return _lazyValueProviderBuilder().Build();
    }
  }
}