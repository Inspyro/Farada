using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  /// <summary>
  /// TODO
  /// </summary>
  public class FuncProvider<T>:ValueProvider<T>
  {
    private readonly Func<ValueProviderContext<T>, T> _valueFunc;

    public FuncProvider (Func<ValueProviderContext<T>, T> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override T CreateValue (ValueProviderContext<T> context)
    {
      return _valueFunc(context);
    }
  }
}