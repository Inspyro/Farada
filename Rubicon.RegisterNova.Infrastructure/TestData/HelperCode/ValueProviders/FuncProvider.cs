using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  public class FuncProvider<T>:ValueProvider<T>
  {
    private readonly Func<ValueProviderContext, T> _valueFunc;

    public FuncProvider (Func<ValueProviderContext, T> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override T CreateValue ()
    {
      return _valueFunc(Context);
    }
  }
}