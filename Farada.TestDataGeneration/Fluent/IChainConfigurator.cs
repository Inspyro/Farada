using System;
using System.Linq.Expressions;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IChainConfigurator
  {
    IValueProviderAndChainConfigurator<TMember> For<TMember> ();
    // TODO: T"Container"?
    IValueProviderAndChainConfigurator<TContainer, TMember> For<TContainer, TMember> (Expression<Func<TContainer, TMember>> memberExpression);
  }
}