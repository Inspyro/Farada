using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Modifiers;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// Builds a <see cref="CompoundValueProvider"/> based on the specified chains
  /// </summary>
  internal class CompoundValueProviderBuilder : ICompoundValueProviderBuilder
  {
    private readonly IRandom _random;
    private readonly IParameterConversionService _parameterConversionService;
    private readonly ValueProviderDictionary _valueProviderDictionary;
    private readonly HashSet<IKey> _autoFillMapping;
    private readonly IList<IInstanceModifier> _modifierList;

    internal CompoundValueProviderBuilder(IRandom random, IParameterConversionService parameterConversionService)
    {
      _random = random;
      _parameterConversionService = parameterConversionService;
      _valueProviderDictionary = new ValueProviderDictionary();
      _autoFillMapping = new HashSet<IKey>();
      _modifierList = new List<IInstanceModifier>();
    }

    public void AddProvider<TMember, TContext> (ValueProvider<TMember, TContext> valueProvider) where TContext : ValueProviderContext<TMember>
    {
      _valueProviderDictionary.AddValueProvider(new TypeKey(typeof (TMember)), valueProvider);
    }

    public void AddProvider<TMember, TAttribute, TContext> (AttributeBasedValueProvider<TMember, TAttribute, TContext> attributeBasedValueProvider) where TAttribute : Attribute where TContext : AttributeValueProviderContext<TMember, TAttribute>
    {
      var key = new AttributeKey(typeof (TMember), typeof (TAttribute));
      _valueProviderDictionary.AddValueProvider(key, attributeBasedValueProvider);
    }

    public void AddProvider<TMember, TContainer, TContext> (
        Expression<Func<TContainer, TMember>> chainExpression,
        ValueProvider<TMember, TContext> valueProvider) where TContext : ValueProviderContext<TMember>
    {
      _valueProviderDictionary.AddValueProvider (GetChainedKey (chainExpression), valueProvider);
    }

    private static ChainedKey GetChainedKey<TMember, TContainer> (Expression<Func<TContainer, TMember>> chainExpression)
    {
      var declaringType = chainExpression.GetParameterType();
      var expressionChain = chainExpression.ToChain().ToList();

      if (expressionChain.Count == 0)
        throw new ArgumentException ("Empty chains are not supported, please use AddProvider<T>()");

      var chainedKey = new ChainedKey (declaringType, expressionChain);
      return chainedKey;
    }

    public void AddInstanceModifier (IInstanceModifier instanceModifier)
    {
      _modifierList.Add(instanceModifier);
    }

    public void DisableAutoFill<TType>()
    {
      DisableAutoFill(new TypeKey(typeof(TType)));
    }

    public void DisableAutoFill<TMember, TContainer> (Expression<Func<TContainer, TMember>> chainExpression)
    {
      DisableAutoFill (GetChainedKey (chainExpression));
    }
    private void DisableAutoFill(IKey key)
    {
      if (_autoFillMapping.Contains (key))
        throw new InvalidOperationException ("The key " + key + " was already disabled for auto fill.");

      _autoFillMapping.Add(key);
    }

    internal CompoundValueProvider Build()
    {
      return new CompoundValueProvider(_valueProviderDictionary, _autoFillMapping, _random, _modifierList, _parameterConversionService);
    }
  }
}