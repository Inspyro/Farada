using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// Resolves properties from intances based on the member information in the provided keys.
  /// </summary>
  internal class DependendPropertyResolver
  {
    public IEnumerable<DependedPropertyCollection> ResolveDependendProperties (SubTypeInstanceHolder instancesForType, IList<IKey> dependencies)
    {
      foreach (var instance in instancesForType.Instances)
      {
        var dependendPropertyCollection = new DependedPropertyCollection();
        foreach (var dependency in dependencies)
        {
          if (dependency.Member == null)
            throw new ArgumentException ("Did not find member for dependency.. todo");

          dependendPropertyCollection.Add (dependency, dependency.Member.GetValue (instance));
        }

        yield return dependendPropertyCollection;
      }
    }
  }
}