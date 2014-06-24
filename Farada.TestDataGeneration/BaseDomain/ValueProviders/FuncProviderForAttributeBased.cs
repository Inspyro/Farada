using System;
using Farada.TestDataGeneration.ValueProvider;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// TODO
  /// </summary>
  public class FuncProviderForAttributeBased<TProperty, TAttribute>:AttributeBasedValueProvider<TProperty, TAttribute>
    where TAttribute : Attribute
  {
    private readonly Func<AttributeValueProviderContext<TProperty, TAttribute>, TProperty> _valueFunc;

    public FuncProviderForAttributeBased (Func<AttributeValueProviderContext<TProperty, TAttribute>, TProperty> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override TProperty CreateValue (AttributeValueProviderContext<TProperty, TAttribute> context)
    {
      return _valueFunc(context);
    }
  }
}