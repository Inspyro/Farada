using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration
{
  public abstract class ValueProvider<TProperty>:IValueProvider
  {
    protected ValueProviderContext Context { get; private set; }

    public object GetObjectValue (ValueProviderContext context)
    {
      Context = context;
      return GetValue();
    }

    protected abstract TProperty GetValue ();
  }
}
