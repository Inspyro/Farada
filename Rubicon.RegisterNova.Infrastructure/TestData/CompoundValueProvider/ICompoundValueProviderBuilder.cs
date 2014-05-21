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
    void AddProvider<TProperty> (ValueProvider<TProperty> valueProvider);

    /// <summary>
    /// TODO
    /// </summary>
    void AddProvider<TProperty, TAttribute> (AttributeValueProvider<TProperty, TAttribute> attributeValueProvider) where TAttribute : Attribute;

    /// <summary>
    /// TODO
    /// </summary>
    void AddProvider<TProperty, TContainer> (Expression<Func<TContainer, TProperty>> chainExpression, ValueProvider<TProperty> valueProvider);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="instanceModifier"></param>
    void AddInstanceModifier (IInstanceModifier instanceModifier);
  }
}