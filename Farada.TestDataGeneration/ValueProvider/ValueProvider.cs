using System;
using AutoMapper.Internal;

namespace Farada.TestDataGeneration.ValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  /// <typeparam name="TProperty">TODO</typeparam>
  /// <typeparam name="TContext"></typeparam>
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
    /// TODO
    /// </summary>
    /// <param name="context"></param>
    protected abstract TProperty CreateValue (TContext context);
  }

  public abstract class ValueProvider<TProperty> : ValueProvider<TProperty, ValueProviderContext<TProperty>>
  {
    protected override ValueProviderContext<TProperty> CreateContext (ValueProviderObjectContext objectContext)
    {
      return new ValueProviderContext<TProperty>(objectContext);
    }
  }

}