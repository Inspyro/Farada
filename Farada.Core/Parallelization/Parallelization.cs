using System;
using System.Collections.Generic;
using System.Linq;
using Farada.Core.Extensions;

namespace Farada.Core.Parallelization
{
  public static class Parallelization
  {
    public static IEnumerable<T> DistributeParallel<T>(Func<int, IEnumerable<T>> enumerableFunc, int count)
    {
      var threadCount = Environment.ProcessorCount*2;
      var countPerThread = (int) ((double) count / threadCount);

      return EnumerableExtensions.RepeatParallel(() => enumerableFunc(countPerThread).ToList(), threadCount, threadCount).SelectMany(list => list);
    }
  }
}
