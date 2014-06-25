using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Extensions;

namespace Farada.TestDataGeneration.Parallelization
{
  /// <summary>
  /// This class provides functionality for parallelization
  /// </summary>
  public static class Parallelization
  {
    /// <summary>
    /// Parallelly distributes the execution of a func that creates an <see cref="IEnumerable{T}"/> and combines it into a resulting IEnumerable
    /// This method could be used together with the <see cref="ITestDataGenerator.CreateMany{TCompoundValue}"/> method for example
    /// </summary> 
    /// <typeparam name="T">The type of the iEnumerable</typeparam>
    /// <param name="enumerableFunc">The func the creates one IEnumerable based on the given chunckSize for each thread</param>
    /// <param name="count">The size of the resulting IEnumerable</param>
    /// <returns>The combined IEnumerable</returns>
    public static IEnumerable<T> DistributeParallel<T>(Func<int, IEnumerable<T>> enumerableFunc, int count)
    {
      var threadCount = Environment.ProcessorCount*2;
      var countPerThread = (int) ((double) count / threadCount);

      return EnumerableExtensions.RepeatParallel(() => enumerableFunc(countPerThread).ToList(), threadCount, threadCount).SelectMany(list => list);
    }
  }
}
