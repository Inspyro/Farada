using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rubicon.RegisterNova.Infrastructure.Utilities
{
  public static class ListExtensions
  {
    /// <summary>
    /// Selects a random value from a list that matches the predicate but does not match the excluded values
    /// Performance: O(n)
    /// </summary>
    /// <typeparam name="T">The inner type of the list</typeparam>
    /// <param name="list">The list to search</param>
    /// <param name="random"></param>
    /// <returns>A random value or default(T) if no value matching predicate and excludes was found</returns>
    /*public static T RandomValue<T>(this IList<T> list, Func<T, bool> predicate, T[] excludes, Random random)
    {
      var possibleIndices= list.WhereIndices(predicate).ToList();
      
      if(possibleIndices.Count<=0)
        return default (T);

      var initialIndex = possibleIndices.RandomIndex(random);

      if(excludes==null)
      {
        return list[possibleIndices[initialIndex]];
      }

      var currentIndex = initialIndex;

      var countUp = true;
      T finalValue;
      while (excludes.Contains(finalValue = list[possibleIndices[currentIndex]]))
      {
        if (countUp)
        {
          var isLastItem = currentIndex >= possibleIndices.Count - 1;
          if(isLastItem)
          {
            countUp = false;
            currentIndex = initialIndex;
          }
          else
          {
            currentIndex++;
          }
        }
       
        if(!countUp)
        {
          if (currentIndex == 0)
            break;

          currentIndex--;
        }
      }

      return finalValue;
    }*/

    /// <summary>
    /// Randomly selects an index from a list
    /// </summary>
    /// <returns>A random index from the list</returns>
    /*public static int RandomIndex<T>(this IList<T> list, Random random)
    {
      if (list.Count <= 0)
        throw new ArgumentOutOfRangeException("list", "You cannot randomly select a index of an empty list");

      return random.Next(0, list.Count);
    }*/

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
