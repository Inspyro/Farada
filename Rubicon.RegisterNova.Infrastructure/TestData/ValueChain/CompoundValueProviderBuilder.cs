using System;
using System.Linq;
using System.Linq.Expressions;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class CompoundValueProviderBuilder
  {
    private readonly Random _random;
    private readonly ValueProviderDictionary _valueProviderDictionary;

    public CompoundValueProviderBuilder(Random random)
    {
      _random = random;
      _valueProviderDictionary = new ValueProviderDictionary();
    }

    public void SetProvider<TProperty>(ValueProvider<TProperty> valueProvider)
    {
      var key = new Key(new KeyPart (typeof (TProperty) ));
      _valueProviderDictionary.SetValueProvider(key, valueProvider);
    }

    public void SetProvider<TProperty, TContainer>(Expression<Func<TContainer, TProperty>> chainExpression, ValueProvider<TProperty> valueProvider)
    {
      var expressionChain = chainExpression.ToChain().ToList();

      var key = new Key(expressionChain);
      _valueProviderDictionary.SetValueProvider(key, valueProvider);
    }

    internal CompoundValueProvider Build()
    {
      return new CompoundValueProvider(_valueProviderDictionary, _random); //TODO: rename to ValueProviderChain
    }
  }
}