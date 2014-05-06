using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode
{
  public class FuncProvider<T>:ValueProvider<T>
  {
    private readonly Func<RandomGenerator<T>, T> _valueFunc;

    public FuncProvider (Func<RandomGenerator<T>, T> valueFunc)
        : base(null)
    {
      _valueFunc = valueFunc;
    }

    protected override T GetValue (T currentValue)
    {
      return _valueFunc(RandomGenerator);
    }
  }
}