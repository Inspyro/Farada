using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Allows to define a func instead of deriving from <see cref="ValueProvider{TMember}"/>
  /// In order to simplify this process even more one can use the <see cref="IProviderConfiguratorExtensions"/>
  /// </summary>
  public class FuncProvider<TContainer, TMember> : ValueProviderWithContainer<TContainer, TMember>
  {
    private readonly Func<ValueProviderContext<TContainer, TMember>, TMember> _valueFunc;

    /// <summary>
    /// Create a func value provider
    /// </summary>
    /// <param name="valueFunc">the func that creates the value based on a <see cref="ValueProviderContext{TMember}"/></param>
    public FuncProvider (Func<ValueProviderContext<TContainer, TMember>, TMember> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override TMember CreateValue (ValueProviderContext<TContainer, TMember> context)
    {
      return _valueFunc (context);
    }
  }

  public class FuncProvider<TMember> : ValueProvider<TMember>
  {
    private readonly Func<ValueProviderContext<TMember>, TMember> _valueFunc;

    /// <summary>
    /// Create a func value provider
    /// </summary>
    /// <param name="valueFunc">the func that creates the value based on a <see cref="ValueProviderContext{TMember}"/></param>
    public FuncProvider(Func<ValueProviderContext<TMember>, TMember> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override TMember CreateValue(ValueProviderContext<TMember> context)
    {
      return _valueFunc(context);
    }
  }


  /// <summary>
    /// Allows to define a func instead of deriving from <see cref="ValueProvider{TMember}"/>
    /// In order to simplify this process even more one can use the <see cref="IProviderConfiguratorExtensions"/>
    /// </summary>
  public class AttributeFuncProvider<TMember, TAttribute> : AttributeBasedValueProvider<TMember, TAttribute>
      where TAttribute : Attribute
  {
    private readonly Func<ExtendedValueProviderContext<TMember, IList<TAttribute>>, TMember> _valueFunc;

    /// <summary>
    /// Create a func value provider
    /// </summary>
    /// <param name="valueFunc">the func that creates the value based on a <see cref="ValueProviderContext{TMember}"/></param>
    public AttributeFuncProvider(Func<ExtendedValueProviderContext<TMember, IList<TAttribute>>, TMember> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override TMember CreateAttributeBasedValue (ExtendedValueProviderContext<TMember, IList<TAttribute>> context)
    {
      return _valueFunc (context);
    }
  }

  public class AttributeFuncProvider<TContainer, TMember, TAttribute> : AttributeBasedValueProvider<TContainer, TMember, TAttribute>
      where TAttribute : Attribute
  {
    private readonly Func<ExtendedValueProviderContext<TContainer, TMember, IList<TAttribute>>, TMember> _valueFunc;

    /// <summary>
    /// Create a func value provider
    /// </summary>
    /// <param name="valueFunc">the func that creates the value based on a <see cref="ValueProviderContext{TMember}"/></param>
    public AttributeFuncProvider(Func<ExtendedValueProviderContext<TContainer, TMember, IList<TAttribute>>, TMember> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override TMember CreateAttributeBasedValue(ExtendedValueProviderContext<TContainer, TMember, IList<TAttribute>> context)
    {
      return _valueFunc(context);
    }
  }
}