using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration
{
  public abstract class ValueProvider<TProperty>:IValueProvider
  {
    private readonly bool _wantsPreviousValue;

    protected TProperty CurrentValue { get; private set; }
    protected Random Random { get; private set; }

     protected ValueProvider (bool wantsPreviousValue=false)
     {
       _wantsPreviousValue = wantsPreviousValue;
     }

    public object GetObjectValue (Random random, object currentValue)
    {
      Random = random;
      CurrentValue = (TProperty) currentValue;
      return GetValue();
    }

    public bool WantsPreviousValue()
    {
      return _wantsPreviousValue;
    }

    protected abstract TProperty GetValue ();
  }
}
