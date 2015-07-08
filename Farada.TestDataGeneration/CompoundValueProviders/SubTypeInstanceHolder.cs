using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
    /// <summary>
    /// This class represents a list of concrete subtypes of a base type.
    /// </summary>
    internal class SubTypeInstanceHolder
    {
        internal IKey Key { get; private set; }
        internal IList<object> Instances { get; private set; }  

        private SubTypeInstanceHolder (IKey currentKey, IList<object> instances=null)
        {
            Key = currentKey;
            Instances = instances??new List<object>();
        }

        /// <summary>
        /// This methods splits the list of objects into their correct subtypes (based on the given key)
        /// </summary>
        /// <param name="currentKey">The key for the instances e.g. Dog.Cat of type Animal</param>
        /// <param name="instances">The concrete instances for this property e.g 10 instance of AngoraCat and 10 instances of TigerCat</param>
        /// <returns>a dictionary that maps each concrete type to the corresponding instances and the correct key - so in this case you would get {{AngoraCat, {Cat1, Cat3, Cat 5}}, {TigerCat, {Cat2, Cat4,...}}}</returns>
        internal static Dictionary<Type, SubTypeInstanceHolder> SplitUpSubTypes(IKey currentKey, IList<object> instances)
        {
            //First we check if all instances are of the same concrete type as the property (e.g. only Animals)
            if (instances.Where(instance => instance != null).All(instance => instance.GetType() == currentKey.Type))
            {
                //if this is the case we return a dictionary with just this type (for convenience)
                return new Dictionary<Type, SubTypeInstanceHolder> { { currentKey.Type, new SubTypeInstanceHolder(currentKey, instances) } };
            }

            //otherwise we have to get all the concrete types
            var typeToInstances = new Dictionary<Type, SubTypeInstanceHolder>();
            foreach (var instance in instances.Where(instance => instance != null))
            {
                //we get the concrete type of the instance - e.g. AngoraCat
                var concreteType = instance.GetType();

                //now we check if this is the first instance
                if (!typeToInstances.ContainsKey(concreteType))
                {
                    //if it is the first property we add it, but we need to change the type of the key (as this might use other value providers)
                    typeToInstances.Add(
                        concreteType,
                        new SubTypeInstanceHolder(currentKey.ChangeMemberType(concreteType)));
                }

                //if it was not the first instance, we just add it to the old mapping (this is basically for performance reasons, see CompoundValueProvider for more info
                typeToInstances[concreteType].Instances.Add(instance);
            }

            return typeToInstances;
        }
    }
}