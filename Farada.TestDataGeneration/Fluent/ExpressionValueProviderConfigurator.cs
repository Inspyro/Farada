using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  internal class ExpressionValueProviderConfigurator<TContainer,TMember>:ChainConfigurator, IValueProviderAndChainConfigurator<TMember>
  {
    private readonly Expression<Func<TContainer, TMember>> _memberExpression;

    public ExpressionValueProviderConfigurator (Expression<Func<TContainer, TMember>> memberExpression, Func<CompoundValueProviderBuilder> lazyValueProviderBuilder)
        : base(lazyValueProviderBuilder)
    {
      _memberExpression = memberExpression;
    }

    public IValueProviderAndChainConfigurator<TMember> AddProvider (ValueProvider<TMember> valueProvider)
    {
      _lazyValueProviderBuilder().AddProvider(_memberExpression, valueProvider);
      return this;
    }

    public IValueProviderAndChainConfigurator<TMember> AddProvider<TContext> (ValueProvider<TMember, TContext> valueProvider) where TContext : ValueProviderContext<TMember>
    {
      _lazyValueProviderBuilder().AddProvider(_memberExpression, valueProvider);
      return this;
    }

    public IValueProviderAndChainConfigurator<TMember> DisableAutoFill ()
    {
      _lazyValueProviderBuilder().DisableAutoFill (_memberExpression);
      return this;
    }
  }
}