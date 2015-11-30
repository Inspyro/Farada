using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  public interface ITestDataGeneratorAdvanced
  {
    ///Note: The create many method is optimized in many ways
    ///1: If you create 100 Dog objects it will first created 100 instances of Dog (with a fast version of the Activator)
    ///   Then it searches the value provider for each member and generates 100 values per member for each instance of Dog
    /// 2: It uses a cached but thread safe version of reflection <see cref="FastReflectionUtility"/>
    IList<object> CreateMany (IKey currentKey, [CanBeNull] IList<DependedPropertyCollection> dependedProperties, int itemCount, int maxRecursionDepth);
  }
}