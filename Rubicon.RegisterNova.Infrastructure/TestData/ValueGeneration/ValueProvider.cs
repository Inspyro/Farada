using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration
{
  public abstract class ValueProvider<TProperty>:IValueProvider
  {
    protected Random Random { get; private set; }

    protected ValueProvider ()
    {
    }

    public object GetObjectValue (Random random)
    {
      Random = random;
      return GetValue(); //GetConcreteValue(default(TProperty), random);
    }

    /*protected virtual TProperty GetConcreteValue(TProperty currentValue, Random random)
    {
      Random = random;
      return GetNextValue(GetValue(currentValue), random);
    }*/

    protected abstract TProperty GetValue ();

    /*protected TProperty GetNextValue (TProperty currentValue, Random random)
    {
      return _nextProvider != null ? _nextProvider.GetConcreteValue(currentValue, random) : currentValue;
    }*/
  }
}
