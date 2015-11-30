using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Allows to define a func instead of deriving from <see cref="AttributeBasedValueProvider{TMember,TAttribute}"/>
  /// In order to simplify this process even more one can use the <see cref="IProviderConfiguratorExtensions"/>
  /// </summary>
  public class FuncProviderForAttributeBased<TMember, TAttribute>:AttributeBasedValueProvider<TMember, TAttribute>
    where TAttribute : Attribute
  {
    private readonly Func<ExtendedValueProviderContext<TMember, TAttribute>, TMember> _valueFunc;

     /// <summary>
    /// Create a func value provider
    /// </summary>
    /// <param name="valueFunc">the func that creates the value based on a <see cref="AttributeValueProviderContext{TMember, TAttribute}"/></param>
    public FuncProviderForAttributeBased (Func<ExtendedValueProviderContext<TMember, TAttribute>, TMember> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override TMember CreateValue (ExtendedValueProviderContext<TMember, TAttribute> context)
    {
      return _valueFunc(context);
    }
  }
}