using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration
{
  public abstract class ValueProvider<TProperty>:IValueProvider
  {
    private readonly ValueProvider<TProperty> _nextProvider;

    protected ValueProvider(ValueProvider<TProperty> nextProvider)
    {
      _nextProvider = nextProvider;
    }

    protected virtual TProperty GetValue (TProperty currentValue)
    {
      return _nextProvider != null ? _nextProvider.GetValue(currentValue) : currentValue;
    }

    public object GetObjectValue ()
    {
      return GetValue(default(TProperty));
    }
  }
}
