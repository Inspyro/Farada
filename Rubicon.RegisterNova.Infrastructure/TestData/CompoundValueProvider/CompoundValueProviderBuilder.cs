using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  internal class CompoundValueProviderBuilder : ICompoundValueProviderBuilder
  {
    private readonly Random _random;
    private readonly ValueProviderDictionary _valueProviderDictionary;
    private readonly IList<IInstanceModifier> _modifierList;

    internal CompoundValueProviderBuilder(Random random)
    {
      _random = random;
      _valueProviderDictionary = new ValueProviderDictionary();
      _modifierList = new List<IInstanceModifier>();
    }

    public void AddProvider<TProperty>(ValueProvider<TProperty> valueProvider)
    {
      var key = new Key(typeof (TProperty));
      _valueProviderDictionary.AddValueProvider(key, valueProvider);
    }

    public void AddProvider<TProperty, TAttribute> (AttributeValueProvider<TProperty, TAttribute> attributeValueProvider) where TAttribute : Attribute
    {
      var key = new Key(typeof (TAttribute));
      _valueProviderDictionary.AddValueProvider(key, attributeValueProvider);
    }

    public void AddProvider<TProperty, TContainer>(Expression<Func<TContainer, TProperty>> chainExpression, ValueProvider<TProperty> valueProvider)
    {
      var expressionChain = chainExpression.ToChain().ToList();

      var key = new Key(expressionChain);
      _valueProviderDictionary.AddValueProvider(key, valueProvider);
    }


    public void AddInstanceModifier (IInstanceModifier instanceModifier)
    {
      _modifierList.Add(instanceModifier);
    }

    internal CompoundValueProvider Build()
    {
      return new CompoundValueProvider(_valueProviderDictionary, _random, _modifierList);
    }
  }
}