using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Modifiers;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;
using JetBrains.Annotations;

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

    private readonly Dictionary<Type, int> _tempContainerCountMapping; 
    private readonly Dictionary<IKey, int> _containerIndexMapping;

    private Dictionary<IKey, Func<MetadataObjectContext, object>> _metadataProviderMapping;

    internal CompoundValueProviderBuilder (IRandom random, IParameterConversionService parameterConversionService)
    {
      _random = random;
      _parameterConversionService = parameterConversionService;
      _valueProviderDictionary = new ValueProviderDictionary();
      _autoFillMapping = new HashSet<IKey>();
      _modifierList = new List<IInstanceModifier>();

      _tempContainerCountMapping = new Dictionary<Type, int>();
      _containerIndexMapping = new Dictionary<IKey, int>();

      _metadataProviderMapping = new Dictionary<IKey, Func<MetadataObjectContext, object>>();
    }

    public void AddProvider<TContext>(ValueProvider<object, TContext> valueProvider, Type realType) where TContext:ValueProviderContext<object>
    {
      _valueProviderDictionary.AddValueProvider (new TypeKey (realType), valueProvider);
    } 

    public void AddProvider<TMember, TContext> (ValueProvider<TMember, TContext> valueProvider) where TContext : ValueProviderContext<TMember>
    {
      _valueProviderDictionary.AddValueProvider (new TypeKey (typeof (TMember)), valueProvider);
    }

    public void AddProvider<TMember, TContainer, TMetadata, TContext> (
        Expression<Func<TContainer, TMember>> chainExpression,
        Func<BoundMetadataContext<TContainer>, TMetadata> metadataProviderFunc,
        ValueProvider<TMember, TContext> valueProvider) where TContext : ValueProviderContext<TMember>
    {
      var providerKey = ChainedKey.FromExpression (chainExpression);
      _valueProviderDictionary.AddValueProvider (providerKey, valueProvider);
      _containerIndexMapping[providerKey] = GetNextIndexInContainer<TContainer>();

      //here we bend the metadata provider func to our needs :)
      _metadataProviderMapping.Add (providerKey, BendFuncToMetadata (metadataProviderFunc));
    }

    private static Func<MetadataObjectContext, object> BendFuncToMetadata<TContainer, TMetadata> (Func<BoundMetadataContext<TContainer>, TMetadata> metadataProviderFunc)
    {
      return objectContext => metadataProviderFunc (new BoundMetadataContext<TContainer> (objectContext));
    }

    public void AddProvider<TMember, TContainer, TContext> (
        Expression<Func<TContainer, TMember>> chainExpression,
        ValueProvider<TMember, TContext> valueProvider) where TContext : ValueProviderContext<TMember>
    {
      var providerKey = ChainedKey.FromExpression (chainExpression);
      _valueProviderDictionary.AddValueProvider (providerKey, valueProvider);
      _containerIndexMapping[providerKey] = GetNextIndexInContainer<TContainer>();
    }
    private int GetNextIndexInContainer<TContainer>()
    {
      if (!_tempContainerCountMapping.ContainsKey(typeof(TContainer)))
      {
        _tempContainerCountMapping[typeof(TContainer)] = 0;
      }

      var indexInContainer = _tempContainerCountMapping[typeof(TContainer)];
      _tempContainerCountMapping[typeof(TContainer)]++;
      return indexInContainer;
    }

    public void AddInstanceModifier (IInstanceModifier instanceModifier)
    {
      _modifierList.Add (instanceModifier);
    }

    public void EnableAutoFill(Type type)
    {
      EnableAutoFill(new TypeKey(type));
    }

    public void EnableAutoFill<TType> ()
    {
      EnableAutoFill(new TypeKey (typeof (TType)));
    }

    public void EnableAutoFill<TMember, TContainer> (Expression<Func<TContainer, TMember>> chainExpression)
    {
      EnableAutoFill(ChainedKey.FromExpression (chainExpression));
    }

    private void EnableAutoFill(IKey key)
    {
      if (_autoFillMapping.Contains (key))
        throw new InvalidOperationException ("The key " + key + " was already enabled for auto fill.");

      _autoFillMapping.Add (key);
    }

    internal CompoundValueProvider Build ()
    {
      return new CompoundValueProvider (
          _valueProviderDictionary,
          _autoFillMapping,
          new MemberContainerIndexSorter(_containerIndexMapping),
          new MetadataResolver(_metadataProviderMapping),
          _random,
          _modifierList,
          _parameterConversionService);
    }
  }
}