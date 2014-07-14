using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
    internal class SubTypeInstanceHolder
    {
        internal IKey Key { get; private set; }
        internal IList<object> Instances { get; private set; }  

        private SubTypeInstanceHolder (IKey currentKey, IList<object> instances=null)
        {
            Key = currentKey;
            Instances = instances??new List<object>();
        }

        internal static Dictionary<Type, SubTypeInstanceHolder> SplitUpSubTypes(IKey currentKey, IList<object> instances)
        {
            if (instances.Where(instance => instance != null).All(instance => instance.GetType() == currentKey.PropertyType))
            {
                return new Dictionary<Type, SubTypeInstanceHolder> { { currentKey.PropertyType, new SubTypeInstanceHolder(currentKey, instances) } };
            }

            var typeToInstances = new Dictionary<Type, SubTypeInstanceHolder>();
            foreach (var instance in instances.Where(instance => instance != null))
            {
                var concreteType = instance.GetType();
                if (!typeToInstances.ContainsKey(concreteType))
                {
                    typeToInstances.Add(
                        concreteType,
                        new SubTypeInstanceHolder(currentKey.ChangePropertyType(concreteType)));
                }

                typeToInstances[concreteType].Instances.Add(instance);
            }

            return typeToInstances;
        }
    }
}