using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farada.TestDataGeneration.Extensions
{
  /// <summary>
  /// Extensions for <see cref="IEnumerable{T}"/> that help on common tasks
  /// </summary>
  public static class EnumerableExtensions
  {
    /// <summary>
    /// Casts an IEnumerable containing objects to a more specific type, or the default value of T
    /// </summary>
    /// <typeparam name="T">the type of the target ienumerable</typeparam>
    /// <param name="enumerable">the enumerable that contains the object values to cast</param>
    /// <returns>the cast enumerable</returns>
    public static IEnumerable<T> CastOrDefault<T>(this IEnumerable<object> enumerable)
    {
      return enumerable.Select(content => content ?? default(T)).Cast<T>();
    }

    /// <summary>
    /// Creates an <see cref="IEnumerable{T}"/> given a func and a count
    /// </summary>
    /// <typeparam name="T">The type of the resulting IEnumerable</typeparam>
    /// <param name="createT">The func that creates a value of type T for the enumerable</param>
    /// <param name="count">The count how often the func is called and the size of the resulting IEnumerable</param>
    /// <returns>The filled IEnumerable</returns>
    public static IEnumerable<T> Repeat<T>(Func<T> createT, int count)
    {
      for(int i=0;i<count;i++)
      {
        yield return createT();
      }
    }

    /// <summary>
    /// Creates an IEnumerable in parallel threads
    /// </summary>
     /// <typeparam name="T">The type of the resulting IEnumerable</typeparam>
    /// <param name="createT">The func that creates a value of type T for the enumerable</param>
    /// <param name="count">The count how often the func is called and the size of the resulting IEnumerable</param>
    /// <param name="threadCount">The desired count of threads to use, default=0 means it uses the processor count as thread count</param>
   /// <returns>The filled IEnumerable</returns>
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

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, Func<int> randomIntGenerator)
    {
      return enumerable.OrderBy(x => randomIntGenerator());
    }

    /// <summary>
    /// Sorts the items topologically (by dependency)
    /// </summary>
    /// <param name="source">The source enumerable (unsorted).</param>
    /// <param name="dependencies">A mapping between item and dependencies</param>
    /// <param name="throwOnCycle">If true, throws an <see cref="ArgumentException"/> if a cycle is detected.</param>
    /// <returns>The topolocially sorted enumerable.</returns>
    public static IEnumerable<T> TopologicalSort<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> dependencies, bool throwOnCycle = false)
    {
      var sorted = new List<T>();
      var visited = new HashSet<T>();

      foreach (var item in source)
        VisitTopologicItem(item, visited, sorted, dependencies, throwOnCycle);

      return sorted;
    }

    private static void VisitTopologicItem<T>(T item, ISet<T> visited, ICollection<T> sorted, Func<T, IEnumerable<T>> dependencies, bool throwOnCycle)
    {
      if (!visited.Contains(item))
      {
        visited.Add(item);

        foreach (var dep in dependencies(item))
          VisitTopologicItem(dep, visited, sorted, dependencies, throwOnCycle);

        sorted.Add(item);
      }
      else
      {
        if (throwOnCycle && !sorted.Contains(item))
          throw new ArgumentException("Cyclic dependency found");
      }
    }
  }
}
