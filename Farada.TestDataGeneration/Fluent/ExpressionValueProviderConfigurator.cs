using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  internal class ExpressionValueProviderConfigurator<TContainer,TMember>:ChainConfigurator, IValueProviderAndChainConfigurator<TContainer, TMember>
  {
    private readonly Expression<Func<TContainer, TMember>> _memberExpression;

    public ExpressionValueProviderConfigurator (Expression<Func<TContainer, TMember>> memberExpression, Func<CompoundValueProviderBuilder> lazyValueProviderBuilder)
        : base(lazyValueProviderBuilder)
    {
      _memberExpression = memberExpression;
    }

    public IValueProviderAndChainConfigurator<TContainer, TMember> AddProvider (ValueProviderWithContainer<TContainer, TMember> valueProvider, params Expression<Func<TContainer, object>>[] dependencies)
    {
      _lazyValueProviderBuilder().AddProvider(_memberExpression, valueProvider, dependencies);
      return this;
    }

    IValueProviderAndChainConfigurator<TContainer, TMember> IValueProviderConfigurator<TContainer, TMember>.AddProvider<TContext> (
        ValueProvider<TMember, TContext> valueProvider,
        params Expression<Func<TContainer, object>>[] dependencies)
    {
      _lazyValueProviderBuilder().AddProvider (_memberExpression, valueProvider, dependencies);
      return this;
    }

    public IValueProviderAndChainConfigurator<TContainer, TMember> DisableAutoFill ()
    {
      _lazyValueProviderBuilder().DisableAutoFill (_memberExpression);
      return this;
    }
  }
}