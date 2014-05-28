using System;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.Parallelization
{
  public static class Parallelization
  {
    public static IEnumerable<T> DistributeParallel<T>(Func<int, IEnumerable<T>> enumerableFunc, int count)
    {
      var threadNumber = 8;
      var trueCount = (int) ((double) count / threadNumber);

      return EnumerableExtensions.RepeatParallel(() => enumerableFunc(trueCount).ToList(), threadNumber).SelectMany(list => list);
    }
  }
}
