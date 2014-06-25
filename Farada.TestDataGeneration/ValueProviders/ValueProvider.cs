using System;
using AutoMapper.Internal;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Provides a value for a specific type
  /// </summary>
  /// <typeparam name="TProperty">The property type for which the value is created</typeparam>
  /// <typeparam name="TContext">The context under which the value is created</typeparam>
  public abstract class ValueProvider<TProperty, TContext> : IValueProvider
      where TContext : ValueProviderContext<TProperty>
  {
    object IValueProvider.CreateValue (IValueProviderContext context)
    {
      return CreateValue((TContext) context);
    }

    public virtual bool CanHandle (Type propertyType)
    {
      return propertyType == typeof (TProperty) || (propertyType.IsNullableType() && propertyType.GetTypeOfNullable() == typeof (TProperty));
    }

    protected abstract TContext CreateContext (ValueProviderObjectContext objectContext);

    IValueProviderContext IValueProvider.CreateContext (ValueProviderObjectContext objectContext)
    {
      return CreateContext(objectContext);
    }

    /// <summary>
    /// Creates a value of the given property type
    /// </summary>
    /// <param name="context">the concrete context to considre</param>
    protected abstract TProperty CreateValue (TContext context);
  }

  /// <summary>
  /// Like <see cref="ValueProvider{TProperty,TContext}"/> but with the default context which is <see cref="ValueProviderContext{TProperty}"/>
  /// </summary>
  /// <typeparam name="TProperty"></typeparam>
  public abstract class ValueProvider<TProperty> : ValueProvider<TProperty, ValueProviderContext<TProperty>>
  {
    protected override ValueProviderContext<TProperty> CreateContext (ValueProviderObjectContext objectContext)
    {
      return new ValueProviderContext<TProperty>(objectContext);
    }
  }

}