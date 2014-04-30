using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode
{
  internal class FuncProvider<T>:ValueProvider<T>
  {
    private readonly Func<T> _valueFunc;

    public FuncProvider (Func<T> valueFunc)
        : base(null)
    {
      _valueFunc = valueFunc;
    }

    protected override T GetValue (T currentValue)
    {
      return _valueFunc();
    }
  }
}