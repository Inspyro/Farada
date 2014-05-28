using System;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
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
      return propertyType == typeof (TProperty);
    }

    public abstract TContext CreateContext (
        ICompoundValueProvider compoundValueProvider,
        Random random,
        Func<object> getPreviousValue,
        Type propertyType,
        IFastPropertyInfo fastPropertyInfo);

    IValueProviderContext IValueProvider.CreateContext (
        ICompoundValueProvider compoundValueProvider,
        Random random,
        Func<object> getPreviousValue,
        Type propertyType,
        IFastPropertyInfo fastPropertyInfo)
    {
      return CreateContext(compoundValueProvider, random, getPreviousValue, propertyType, fastPropertyInfo);
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="context"></param>
    protected abstract TProperty CreateValue (TContext context);
  }

  public abstract class ValueProvider<TProperty> : ValueProvider<TProperty, ValueProviderContext<TProperty>>
  {
    public override ValueProviderContext<TProperty> CreateContext (
        ICompoundValueProvider compoundValueProvider,
        Random random,
        Func<object> getPreviousValue,
        Type propertyType,
        IFastPropertyInfo fastPropertyInfo)
    {
      return new ValueProviderContext<TProperty>(compoundValueProvider, random, () => (TProperty) getPreviousValue(), propertyType, fastPropertyInfo);
    }
  }

}