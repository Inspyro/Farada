﻿using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  /// <summary>
  /// TODO
  /// </summary>
  public class FuncProviderForAttribute<TProperty, TAttribute>:AttributeValueProvider<TProperty, TAttribute>
    where TAttribute : Attribute
  {
    private readonly Func<AttributeValueProviderContext<TProperty, TAttribute>, TProperty> _valueFunc;

    public FuncProviderForAttribute (Func<AttributeValueProviderContext<TProperty, TAttribute>, TProperty> valueFunc)
    {
      _valueFunc = valueFunc;
    }

    protected override TProperty CreateValue (AttributeValueProviderContext<TProperty, TAttribute> context)
    {
      return _valueFunc(context);
    }
  }
}