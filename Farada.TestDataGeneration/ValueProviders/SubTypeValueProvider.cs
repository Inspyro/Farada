using System;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// A <see cref="IValueProvider"/> that can not only generate properties of type TProperty but also all subtypes
  /// </summary>
  /// <typeparam name="TProperty">The type of the property</typeparam>
  /// <typeparam name="TContext">The type of the context for the property generation</typeparam>
  public abstract class SubTypeValueProvider<TProperty, TContext> : ValueProvider<TProperty, TContext>
      where TContext : ValueProviderContext<TProperty>
  {
    public override bool CanHandle (Type propertyType)
    {
      return typeof (TProperty).IsAssignableFrom(propertyType);
    }
  }

  /// <summary>
  /// A <see cref="IValueProvider"/> that can not only generate properties of type TProperty but also all subtypes
  /// like <see cref="SubTypeValueProvider{TProperty,TContext}"/> but with the default context
  /// which is <see cref="ValueProviderContext{TProperty}"/>
  /// </summary>
  /// <typeparam name="TProperty">The type of the property</typeparam>
  public abstract class SubTypeValueProvider<TProperty> : ValueProvider<TProperty>
  {
    public override bool CanHandle (Type propertyType)
    {
      return typeof (TProperty).IsAssignableFrom(propertyType);
    }
  }
}