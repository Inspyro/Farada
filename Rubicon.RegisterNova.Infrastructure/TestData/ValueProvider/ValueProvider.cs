using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  /// <typeparam name="TProperty">TODO</typeparam>
  public abstract class ValueProvider<TProperty> : IValueProvider
  {
    protected ValueProviderContext Context { get; private set; }

    public object CreateObjectValue (ValueProviderContext context)
    {
      Context = context;
      return CreateValue();
    }

    /// <summary>
    /// TODO
    /// </summary>
    protected abstract TProperty CreateValue ();
  }
}