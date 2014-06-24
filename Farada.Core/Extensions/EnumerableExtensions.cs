using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farada.Core.Extensions
{
  public static class EnumerableExtensions
  {
    public static IEnumerable<T> CastOrDefault<T>(this IEnumerable<object> enumerable)
    {
      return enumerable.Select(content => content ?? default(T)).Cast<T>();
    }

    public static IEnumerable<T> Repeat<T>(Func<T> createT, int count)
    {
      for(int i=0;i<count;i++)
      {
        yield return createT();
      }
    }

    public static IEnumerable<T> RepeatParallel<T> (Func<T> createT, int count, int threadCount = 0)
    {
      threadCount = threadCount > 0 ? threadCount : Environment.ProcessorCount;
      var countPerThread = (int) ((double) count / threadCount);

      var listOfLists = Repeat(() => new List<T>(), threadCount).ToList();

      var tasks = new Task[threadCount];
      for (var i = 0; i < threadCount; i++)
      {
        var list = listOfLists[i];

        tasks[i] = Task.Factory.StartNew(
            () =>
            {
              for (var c = 0; c < countPerThread; c++)
              {
                list.Add(createT());
              }
            });
      }

      Task.WaitAll(tasks);

      return listOfLists.SelectMany(list => list);
    }
  }
}
