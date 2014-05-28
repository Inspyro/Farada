using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  /// <typeparam name="TProperty">TODO</typeparam>
  /// <typeparam name="TContext"></typeparam>
  public abstract class SubTypeValueProvider<TProperty, TContext> : ValueProvider<TProperty, TContext>
      where TContext : ValueProviderContext<TProperty>
  {
    public override bool CanHandle (Type propertyType)
    {
      return propertyType.IsAssignableFrom(typeof (TProperty));
    }
  }

  public abstract class SubTypeValueProvider<TProperty> : ValueProvider<TProperty>
  {
    public override bool CanHandle (Type propertyType)
    {
      return propertyType.IsAssignableFrom(typeof (TProperty));
    }
  }
}