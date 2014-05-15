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
    protected ValueProviderContext<TProperty> Context { get; private set; }

    internal void CreateContext()
    {

    }

    public object CreateObjectValue (IValueProviderContext context)
    {
      Context = (ValueProviderContext<TProperty>) context;
      return CreateValue();
    }

    /// <summary>
    /// TODO
    /// </summary>
    protected abstract TProperty CreateValue ();

    public IValueProviderContext CreateContext (
        ICompoundValueProvider compoundValueProvider,
        Random random,
        Func<object> getPreviousValue,
        IFastPropertyInfo fastPropertyInfo)
    {
      return new ValueProviderContext<TProperty>(compoundValueProvider, random, () => (TProperty) getPreviousValue(), fastPropertyInfo);
    }
  }
}