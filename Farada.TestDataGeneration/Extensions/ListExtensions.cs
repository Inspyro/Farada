using System;
using System.Collections.Generic;
using System.Linq;

namespace Farada.TestDataGeneration.Extensions
{
  /// <summary>
  /// Common extensions for a <see cref="IList{T}"/>
  /// </summary>
  public static class ListExtensions
  {
    /// <summary>
    /// Randomizes the list inplace with a Fisher-Yates shuffle
    /// </summary>
    /// <typeparam name="T">The type of the list to randomize</typeparam>
    /// <param name="list">The list to randomize</param>
    /// <param name="random">The random to use for shuffeling (if you want to use a seed, etc...), default=null means a new Random is used</param>
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

    /// <summary>
    /// Slices count elements from the beginning of the list
    /// </summary>
    /// <typeparam name="T">The type of the list</typeparam>
    /// <param name="list">The list to slice</param>
    /// <param name="count">The count to slice from the list</param>
    /// <returns>a new list that is sliced by the given count</returns>
    public static List<T> Slice<T>(this IEnumerable<T> list, int count)
    {
      var slicedList = new List<T>(list);
      slicedList.RemoveRange(0, count);

      return slicedList;
    }

    
    

   /// <summary>
    /// This method selects all values of a list that match the given predicate
    /// Performance: O(n)
    /// </summary>
    /// <param name="list">The list to search</param>
    /// <param name="predicate">The predicate (optional), default=null means no criteria</param>
    /// <returns>all values that match the predicate</returns>

    // ReSharper disable once ParameterTypeCanBeEnumerable.Global
    public static IEnumerable<T> WhereValues<T> (this IList<T> list, Func<T, bool> predicate)
    {
      return list.WhereIndices(predicate).Select(index => list[index]);
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
