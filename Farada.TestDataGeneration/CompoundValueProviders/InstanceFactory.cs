using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.Exceptions;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.ValueProviders;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// The instance factory builds instances based on the given <see cref="ValueProviderDictionary"/> which is build by the chain configuration <see cref="CompoundValueProviderBuilder"/>
  /// </summary>
  internal class InstanceFactory
  {
    private readonly CompoundValueProvider _compoundValueProvider;
    private readonly ValueProviderDictionary _valueProviderDictionary;
    private readonly IParameterConversionService _parameterConversionService;

    public InstanceFactory (
        CompoundValueProvider compoundValueProvider,
        ValueProviderDictionary valueProviderDictionary,
        IParameterConversionService parameterConversionService)
    {
      _compoundValueProvider = compoundValueProvider;
      _valueProviderDictionary = valueProviderDictionary;
      _parameterConversionService = parameterConversionService;
    }

    internal IList<object> CreateInstances (IKey key, int numberOfObjects)
    {
      var rootLink = _valueProviderDictionary.GetLink (key);
      return CreateInstances (key, rootLink?.Value, CreateValueProviderContext (rootLink, key), numberOfObjects);
    }

    private IList<object> CreateInstances (
        IKey key,
        [CanBeNull] IValueProvider valueProvider,
        [CanBeNull] IValueProviderContext valueProviderContext,
        int numberOfObjects)
    {
      if (valueProvider == null || valueProviderContext == null)
        throw new MissingValueProviderException ("No value provider registered for \"" + key + "\"");

      return valueProvider.CreateMany (valueProviderContext, numberOfObjects).ToList();
    }

    [CanBeNull]
    private IValueProviderContext CreateValueProviderContext ([CanBeNull] ValueProviderLink providerLink, IKey key)
    {
      if (providerLink == null)
        return null;

      var previousLink = providerLink.Previous?.Invoke();
      var previousContext = previousLink == null ? null : CreateValueProviderContext (previousLink, key);

      return providerLink.Value.CreateContext (
          new ValueProviderObjectContext (
              _compoundValueProvider,
              () =>
              {
                if(previousLink==null)
                {
                  throw new MissingValueProviderException (
                      "Tried to call previous provider on " + key
                      + " but no previous provider was registered. Are you missing a value provider registration?");
                }

                return CreateInstances (previousLink.Key, previousLink.Value, previousContext, 1).Single();
              },
              key.Type,
              new ValueProviderObjectContext.AdvancedContext (key, _parameterConversionService, _compoundValueProvider),
              key.Member));
    }
  }
}