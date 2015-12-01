using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
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
    private readonly Dictionary<IKey, IList<IKey>> _dependencyMapping;
    private readonly IList<IInstanceModifier> _modifierList;

    internal CompoundValueProviderBuilder (IRandom random, IParameterConversionService parameterConversionService)
    {
      _random = random;
      _parameterConversionService = parameterConversionService;
      _valueProviderDictionary = new ValueProviderDictionary();
      _autoFillMapping = new HashSet<IKey>();
      _dependencyMapping = new Dictionary<IKey, IList<IKey>>();
      _modifierList = new List<IInstanceModifier>();
    }

    public void AddProvider<TMember, TContext> (ValueProvider<TMember, TContext> valueProvider) where TContext : ValueProviderContext<TMember>
    {
      _valueProviderDictionary.AddValueProvider (new TypeKey (typeof (TMember)), valueProvider);
    }

    public void AddProvider<TMember, TContainer, TContext> (
        Expression<Func<TContainer, TMember>> chainExpression,
        ValueProvider<TMember, TContext> valueProvider,
        Expression<Func<TContainer, object>>[] dependencies) where TContext : ValueProviderContext<TMember>
    {
      var providerKey = ChainedKey.FromExpression (chainExpression);
      var dependencyKeys = dependencies.Select (ChainedKey.FromExpression).Cast<IKey>().ToList();

      var invalidKey = dependencyKeys.Cast<ChainedKey>().FirstOrDefault (c => c.ChainLength != 1);
      if (invalidKey != null)
      {
        throw new ArgumentException (
            "Chain '" + invalidKey
            + "' is invalid. You can only add dependencies with a chain of length 1. 'Deep Property dependencies' are not supported at the moment.");
      }

      _valueProviderDictionary.AddValueProvider (providerKey, valueProvider);

      //it's possible to add multiple providers per "key".
      if (!_dependencyMapping.ContainsKey (providerKey))
      {
        _dependencyMapping.Add (providerKey, dependencyKeys);
      }
      else
      {
        foreach (var dependencyKey in dependencyKeys)
          _dependencyMapping[providerKey].Add (dependencyKey);
      }
    }

    public void AddInstanceModifier (IInstanceModifier instanceModifier)
    {
      _modifierList.Add (instanceModifier);
    }

    public void DisableAutoFill<TType> ()
    {
      DisableAutoFill (new TypeKey (typeof (TType)));
    }

    public void DisableAutoFill<TMember, TContainer> (Expression<Func<TContainer, TMember>> chainExpression)
    {
      DisableAutoFill (ChainedKey.FromExpression (chainExpression));
    }

    private void DisableAutoFill (IKey key)
    {
      if (_autoFillMapping.Contains (key))
        throw new InvalidOperationException ("The key " + key + " was already disabled for auto fill.");

      _autoFillMapping.Add (key);
    }

    internal CompoundValueProvider Build ()
    {
      return new CompoundValueProvider (
          _valueProviderDictionary,
          _autoFillMapping,
          _dependencyMapping,
          _random,
          _modifierList,
          _parameterConversionService);
    }
  }
}