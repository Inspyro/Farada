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
        private readonly ITestDataGenerator _testDataGenerator;
        private readonly ValueProviderDictionary _valueProviderDictionary;

        public InstanceFactory(ITestDataGenerator testDataGenerator, ValueProviderDictionary valueProviderDictionary)
        {
            _testDataGenerator = testDataGenerator;
            _valueProviderDictionary = valueProviderDictionary;
        }

        internal IList<object> CreateInstances (IKey key, int numberOfObjects)
        {
            var rootLink = _valueProviderDictionary.GetLink(key);
            return CreateInstances(key, rootLink == null ? null : rootLink.Value, CreateValueProviderContext(rootLink, key), numberOfObjects);
        }

        private static IList<object> CreateInstances (IKey key,  [CanBeNull] IValueProvider valueProvider,  [CanBeNull] IValueProviderContext valueProviderContext, int numberOfObjects)
        {
            if (valueProvider == null || valueProviderContext == null)
            {
              //TODO RN-246: Instantiate complex ctors...
                if (!key.Type.CanBeInstantiated())
                    return null;

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
                            _testDataGenerator,
                            () => previousLink == null ? null : CreateInstances(previousLink.Key, previousLink.Value, previousContext, 1).Single(),
                            key.Type,
                            key.Property));
        }
    }
}