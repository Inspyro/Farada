using System;
using System.Linq.Expressions;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  public interface ICompoundValueProviderBuilder
  {
    /// <summary>
    /// TODO
    /// </summary>
    void SetProvider<TProperty> (ValueProvider<TProperty> valueProvider);

    /// <summary>
    /// TODO
    /// </summary>
    void SetProvider<TProperty, TContainer> (Expression<Func<TContainer, TProperty>> chainExpression, ValueProvider<TProperty> valueProvider);
  }
}