using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// Generates random instances of any type that is supported by the concrete test data generator
  /// </summary>
  public interface ITestDataGenerator : ITestDataGeneratorAdvanced
  {
    /// <summary>
    /// Creates exactly one value for the specified type
    /// If you want to create multiple values use <see cref="CreateMany{TCompoundValue}"/> for performance reasons
    /// </summary>
    /// <typeparam name="TCompoundValue">The type to create randomly</typeparam>
    /// <param name="maxRecursionDepth">The maximum recursion depth if a type contains itself e.g. Person->p->p - so when to stop <see cref="RecursionDepth"/></param>
    /// <param name="member">The <see cref="IFastMemberWithValues"/> where the creation starts -> default=null means that we start on the global level</param>
    /// <returns>the randomly created value</returns>
    TCompoundValue Create<TCompoundValue> (int maxRecursionDepth = 2, IFastMemberWithValues member=null);

    /// <summary>
    /// Just like <see cref="Create{TCompoundValue}"/> but creates many objects in an optimized way
    /// </summary>
   /// <typeparam name="TCompoundValue">The type to create randomly</typeparam>
    /// <param name="numberOfObjects">The count of objets to create. Note: If you have a big hierarchy you should not create too many objects, as this will blow up your RAM - You can try to run with ServerGC and 64bit to avoid RAM issues on large data sets</param>
     /// <param name="maxRecursionDepth">The maximum recursion depth if a type contains itself e.g. Person->p->p - so when to stop <see cref="RecursionDepth"/></param>
    /// <param name="member">The <see cref="IFastMemberWithValues"/> where the creation starts -> default=null means that we start on the global level</param>
    /// <returns>the randomly created values</returns>
    IReadOnlyList<TCompoundValue> CreateMany<TCompoundValue> (int numberOfObjects, int maxRecursionDepth = 2, IFastMemberWithValues member=null);

    /// <summary>
    /// The random that is used by the test data generator
    /// </summary>
    Random Random { get; }
  }
}