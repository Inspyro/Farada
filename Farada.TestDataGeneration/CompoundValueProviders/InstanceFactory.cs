using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
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
      private readonly FastReflectionUtility _fastReflectionUtility;

      public InstanceFactory(CompoundValueProvider compoundValueProvider, ValueProviderDictionary valueProviderDictionary, FastReflectionUtility fastReflectionUtility)
        {
            _compoundValueProvider = compoundValueProvider;
            _valueProviderDictionary = valueProviderDictionary;
        _fastReflectionUtility = fastReflectionUtility;
        }

        internal IList<object> CreateInstances (IKey key, int numberOfObjects)
        {
            var rootLink = _valueProviderDictionary.GetLink(key);
            return CreateInstances(key, rootLink == null ? null : rootLink.Value, CreateValueProviderContext(rootLink, key), numberOfObjects);
        }

        private IList<object> CreateInstances (IKey key,  [CanBeNull] IValueProvider valueProvider,  [CanBeNull] IValueProviderContext valueProviderContext, int numberOfObjects)
        {
            if (valueProvider == null || valueProviderContext == null)
            {
              //TODO RN-246: Instantiate complex ctors...
              if (!key.Type.CanBeInstantiated())
              {
                var typeInfo=_fastReflectionUtility.GetTypeInfo (key.Type);

                var ctorValuesCollections = new object[numberOfObjects][];
                for (var i = 0; i < ctorValuesCollections.Length; i++)
                {
                  ctorValuesCollections[i] = new object[typeInfo.CtorArguments.Count];
                }

                for (var argumentIndex = 0; argumentIndex < typeInfo.CtorArguments.Count; argumentIndex++)
                {
                  var ctorArgument = typeInfo.CtorArguments[argumentIndex];

                  //Note: Here we have a recursion to the compound value provider. e.g. other immutable types could be a ctor argument
                  var ctorArgumentValues = _compoundValueProvider.CreateMany (key.CreateKey ((IFastPropertyInfo) ctorArgument), numberOfObjects, 2);

                  for (int valueIndex = 0; valueIndex < ctorArgumentValues.Count; valueIndex++)
                  {
                    ctorValuesCollections[valueIndex][argumentIndex] = ctorArgumentValues[valueIndex];
                  }
                }

                var typeFactoryWithArguments=FastActivator.GetFactory (key.Type, typeInfo.CtorArguments);
                return ctorValuesCollections.Select (ctorValues => typeFactoryWithArguments (ctorValues)).ToList();
              }

              var typeFactory = FastActivator.GetFactory(key.Type);
              return EnumerableExtensions.Repeat(typeFactory, numberOfObjects).ToList();
            }

            return EnumerableExtensions.Repeat(() => valueProvider.CreateValue(valueProviderContext), numberOfObjects).ToList();
        }

        private IValueProviderContext CreateValueProviderContext ( [CanBeNull] ValueProviderLink providerLink, IKey key)
        {
            if (providerLink == null)
                return null;

            var previousLink = providerLink.Previous==null?null:providerLink.Previous();
            var previousContext = previousLink == null ? null : CreateValueProviderContext(previousLink, key);

            return providerLink.Value.CreateContext(
                    new ValueProviderObjectContext(
                            _compoundValueProvider,
                            () => previousLink == null ? null : CreateInstances(previousLink.Key, previousLink.Value, previousContext, 1).Single(),
                            key.Type,
                            key.Property));
        }
    }
}