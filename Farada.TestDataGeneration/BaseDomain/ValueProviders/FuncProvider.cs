using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Allows to define a func instead of deriving from <see cref="ValueProvider{TProperty}"/>
  /// In order to simplify this process even more one can use the <see cref="CompoundValueProviderBuilderExtensions"/>
  /// </summary>
  public class FuncProvider<T>:ValueProvider<T>
  {
    private readonly Func<ValueProviderContext<T>, T> _valueFunc;

    /// <summary>
    /// Create a func value provider
    /// </summary>
    /// <param name="valueFunc">the func that creates the value based on a <see cref="ValueProviderContext{TProperty}"/></param>
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