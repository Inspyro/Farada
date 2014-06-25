using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Allows to define a func instead of deriving from <see cref="AttributeBasedValueProvider{TProperty, TAttribute}"/>
  /// In order to simplify this process even more one can use the <see cref="IProviderConfiguratorExtensions"/>
  /// </summary>
  public class FuncProviderForAttributeBased<TProperty, TAttribute>:AttributeBasedValueProvider<TProperty, TAttribute>
    where TAttribute : Attribute
  {
    private readonly Func<AttributeValueProviderContext<TProperty, TAttribute>, TProperty> _valueFunc;

     /// <summary>
    /// Create a func value provider
    /// </summary>
    /// <param name="valueFunc">the func that creates the value based on a <see cref="AttributeValueProviderContext{TProperty, TAttribute}"/></param>
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