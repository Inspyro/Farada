using System;
using System.Linq.Expressions;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IChainConfigurator
  {
    IValueProviderAndChainConfigurator<TMember> For<TMember>();
    IValueProviderAndChainConfigurator<TMember> For<TContainer, TMember> (Expression<Func<TContainer, TMember>> memberExpression);
    IAttributeProviderAndChainConfigurator<TMember, TAttribute> For<TMember, TAttribute>() where TAttribute : Attribute;
  }
}