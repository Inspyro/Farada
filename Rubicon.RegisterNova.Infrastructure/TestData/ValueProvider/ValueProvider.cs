using System;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  /// <typeparam name="TProperty">TODO</typeparam>
  public abstract class ValueProvider<TProperty> : IValueProvider
  {
    public object CreateObjectValue (IValueProviderContext context)
    {
      return CreateValue( (ValueProviderContext<TProperty>) context);
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="context"></param>
    protected abstract TProperty CreateValue (ValueProviderContext<TProperty> context);

    public IValueProviderContext CreateContext (
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