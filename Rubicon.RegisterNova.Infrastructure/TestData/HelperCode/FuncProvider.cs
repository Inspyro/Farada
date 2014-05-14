using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode
{
  public class FuncProvider<T>:ValueProvider<T>
  {
    private readonly Func<ValueProviderContext, T> _valueFunc;

    public FuncProvider (Func<ValueProviderContext, T> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override T GetValue ()
    {
      return _valueFunc(Context);
    }
  }
}