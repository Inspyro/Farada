using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Allows to define a func instead of deriving from <see cref="ValueProvider{TMember}"/>
  /// In order to simplify this process even more one can use the <see cref="IProviderConfiguratorExtensions"/>
  /// </summary>
  public class FuncProvider<T> : ValueProvider<T>
  {
    private readonly Func<ValueProviderContext<T>, T> _valueFunc;

    /// <summary>
    /// Create a func value provider
    /// </summary>
    /// <param name="valueFunc">the func that creates the value based on a <see cref="ValueProviderContext{TMember}"/></param>
    public FuncProvider (Func<ValueProviderContext<T>, T> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override T CreateValue (ValueProviderContext<T> context)
    {
      return _valueFunc (context);
    }
  }

  /// <summary>
  /// Allows to define a func instead of deriving from <see cref="ValueProvider{TMember}"/>
  /// In order to simplify this process even more one can use the <see cref="IProviderConfiguratorExtensions"/>
  /// </summary>
  public class FuncProvider<TMember, TAttribute> : AttributeBasedValueProvider<TMember, TAttribute>
      where TAttribute : Attribute
  {
    private readonly Func<ExtendedValueProviderContext<TMember, IList<TAttribute>>, TMember> _valueFunc;

    /// <summary>
    /// Create a func value provider
    /// </summary>
    /// <param name="valueFunc">the func that creates the value based on a <see cref="ValueProviderContext{TMember}"/></param>
    public FuncProvider (Func<ExtendedValueProviderContext<TMember, IList<TAttribute>>, TMember> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override TMember CreateAttributeBasedValue (ExtendedValueProviderContext<TMember, IList<TAttribute>> context)
    {
      return _valueFunc (context);
    }
  }
}