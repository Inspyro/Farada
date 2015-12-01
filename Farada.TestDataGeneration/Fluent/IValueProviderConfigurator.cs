using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IValueProviderConfigurator<TContainer, TMember>
  {
    IValueProviderAndChainConfigurator<TContainer, TMember> AddProvider (
        ValueProviderWithContainer<TContainer, TMember> valueProvider,
        params Expression<Func<TContainer, object>>[] dependencies);

    IValueProviderAndChainConfigurator<TContainer, TMember> AddProvider<TContext> (
        ValueProvider<TMember, TContext> valueProvider,
        params Expression<Func<TContainer, object>>[] dependencies)
        where TContext : ValueProviderContext<TContainer, TMember>;

    IValueProviderAndChainConfigurator<TContainer, TMember> DisableAutoFill ();
  }

  public interface IValueProviderConfigurator<TMember>
  {
    IValueProviderAndChainConfigurator<TMember> AddProvider (
        ValueProvider<TMember> valueProvider);

    IValueProviderAndChainConfigurator<TMember> AddProvider<TContext> (
        ValueProvider<TMember, TContext> valueProvider)
        where TContext : ValueProviderContext<TMember>;

    IValueProviderAndChainConfigurator<TMember> DisableAutoFill ();
  }
}