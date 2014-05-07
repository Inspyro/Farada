using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rubicon.RegisterNova.Infrastructure.Utilities
{
  public static class ListExtensions
  {
    public static void Randomize<T> (this IList<T> list, Random random = null)
    {
      random = random ?? new Random();
      
      var n = list.Count;
      while (n > 1)
      {
        n--;
        var k = random.Next(n + 1);
        var value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }

    public static IList<T> Slice<T>(this IList<T> list, int count)
    {
      var slicedList = new List<T>(list);
      slicedList.RemoveRange(0, count);

      return slicedList;
    }

    public static IList<object> Adapt(this IList list)
    {
      return list.Cast<object>().ToList();
    }

    public static IEnumerable<T> WhereValues<T> (this IList<T> list, Func<T, bool> predicate, T[] excludes)
    {
      return list.WhereIndices(predicate).Select(index => list[index]).Where(value => !excludes.Contains(value));
    }

    /// <summary>
    /// This method selects all indices of a list that match the given predicate
    /// Performance: O(n)
    /// </summary>
    /// <param name="list">The list to search</param>
    /// <param name="predicate">The predicate (optional), default=null means no criteria</param>
    /// <returns>all indices that match the predicate</returns>
    public static IEnumerable<int> WhereIndices<T>(this IList<T> list, Func<T, bool> predicate=null)
    {
      if (list.Count <= 0)
        yield break;

      if(predicate==null)
      {
        for(int i=0;i<list.Count;i++)
        {
          yield return i;
        }

        yield break;
      }

      var matchingIndices = new List<int>();
      for (var i = 0; i < list.Count; i++)
      {
        if (predicate(list[i]))
        {
          matchingIndices.Add(i);
        }
      }

      if (matchingIndices.Count <= 0)
        yield break;

      foreach (var matchingIndex in matchingIndices)
      {
        yield return matchingIndex;
      }
    }
  }
}
