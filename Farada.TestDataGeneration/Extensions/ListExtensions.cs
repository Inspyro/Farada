using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.ValueProviders;
using JetBrains.Annotations;

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
    public static void Randomize<T> (this IList<T> list, IRandom random)
    {
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
    public static IEnumerable<T> WhereValues<T> (this IEnumerable<T> list, [CanBeNull] Func<T, bool> predicate)
   {
     return predicate==null? list: list.Where (predicate);
   }
  }
}
