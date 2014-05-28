﻿using System;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.Parallelization
{
  public static class Parallelization
  {
    public static IEnumerable<T> DistributeParallel<T>(Func<int, IEnumerable<T>> enumerableFunc, int count)
    {
      var threadCount = Environment.ProcessorCount;
      var countPerThread = (int) ((double) count / threadCount);

      return EnumerableExtensions.RepeatParallel(() => enumerableFunc(countPerThread).ToList(), threadCount, threadCount).SelectMany(list => list);
    }
  }
}
