﻿using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  internal class ExpressionValueProviderConfigurator<TContainer,TProperty>:ChainConfigurator, IValueProviderAndChainConfigurator<TProperty>
  {
    private readonly Expression<Func<TContainer, TProperty>> _memberExpression;

    public ExpressionValueProviderConfigurator (Expression<Func<TContainer, TProperty>> memberExpression, CompoundValueProviderBuilder valueProviderBuilder)
        : base(valueProviderBuilder)
    {
      _memberExpression = memberExpression;
    }

    public IValueProviderAndChainConfigurator<TProperty> AddProvider (ValueProvider<TProperty> valueProvider)
    {
      _valueProviderBuilder.AddProvider(_memberExpression, valueProvider);
      return this;
    }

    public IValueProviderAndChainConfigurator<TProperty> AddProvider<TContext> (ValueProvider<TProperty, TContext> valueProvider) where TContext : ValueProviderContext<TProperty>
    {
      _valueProviderBuilder.AddProvider(_memberExpression, valueProvider);
      return this;
    }
  }
}