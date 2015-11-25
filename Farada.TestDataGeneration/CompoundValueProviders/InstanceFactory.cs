using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.Exceptions;
using Farada.TestDataGeneration.Extensions;
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

    [CanBeNull]
    internal IList<object> CreateInstances (IKey key, int numberOfObjects)
    {
      var rootLink = _valueProviderDictionary.GetLink (key);
      return CreateInstances (key, rootLink?.Value, CreateValueProviderContext (rootLink, key), numberOfObjects);
    }
    
    [CanBeNull]
    private IList<object> CreateInstances (
        IKey key,
        [CanBeNull] IValueProvider valueProvider,
        [CanBeNull] IValueProviderContext valueProviderContext,
        int numberOfObjects)
    {
      if (valueProvider == null || valueProviderContext == null)
      {
        //TODO RN-246: Adapt check to be sure before creating new instances...
        if (key.Type.IsValueType || key.Type == typeof (string))
          return null;

        try
        {
          return CreateNewInstances (key, numberOfObjects);
        }
        catch (NotSupportedException)
        {
          return null;
        }
      }

      return EnumerableExtensions.Repeat (() => valueProvider.CreateValue (valueProviderContext), numberOfObjects).ToList();
    }

    private IList<object> CreateNewInstances (IKey key, int numberOfObjects)
    {
      var typeInfo = FastReflectionUtility.GetTypeInfo (key.Type);

      var ctorValuesCollections = new object[numberOfObjects][];
      for (var i = 0; i < ctorValuesCollections.Length; i++)
      {
        ctorValuesCollections[i] = new object[typeInfo.CtorArguments.Count];
      }

      for (var argumentIndex = 0; argumentIndex < typeInfo.CtorArguments.Count; argumentIndex++)
      {
        var ctorArgument = typeInfo.CtorArguments[argumentIndex];

        //Note: Here we have a recursion to the compound value provider. e.g. other immutable types could be a ctor argument
        var ctorArgumentValues = _compoundValueProvider.CreateMany (
            key.CreateKey (ctorArgument.ToMember (_parameterConversionService)),
            numberOfObjects,
            2);

        if (ctorArgumentValues == null)
        {
          throw new MissingValueProviderException (
                  $"No value provider specified for type '{ctorArgument.Type.FullName}' but needed for creating an object of type '{key.Type.FullName}'.");
        }

        for (var valueIndex = 0; valueIndex < ctorArgumentValues.Count; valueIndex++)
        {
          ctorValuesCollections[valueIndex][argumentIndex] = ctorArgumentValues[valueIndex];
        }
      }

      var typeFactoryWithArguments = FastActivator.GetFactory (key.Type, typeInfo.CtorArguments);
      return ctorValuesCollections.Select (ctorValues => typeFactoryWithArguments (ctorValues)).ToList();
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
              () => previousLink == null ? null : CreateInstances (previousLink.Key, previousLink.Value, previousContext, 1)?.Single(),
              key.Type,
              key.Member));
    }
  }
}