﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.JetBrainsAnnotations;

namespace Rubicon.RegisterNova.Infrastructure.Utilities
{
  /// <summary>
  /// Extensions methods for <see cref="IEnumerable{T}"/>.
  /// </summary>
  public static class EnumerableExtensions
  {
    public static IEnumerable<T> Repeat<T>(Func<T> createT, int count)
    {
      for(int i=0;i<count;i++)
      {
        yield return createT();
      }
    }

    /// <summary>
    /// Splits an enumerable into chunks with a maximum length of <param name="batchSize"/>.
    /// </summary>
    public static IEnumerable<IList<T>> Batch<T> (this IEnumerable<T> source, int batchSize)
    {
      Trace.Assert(batchSize > 0);
      var sourceAsList = source.ToArray();

      for (var startIndex = 0; startIndex < sourceAsList.Length; startIndex += batchSize)
      {
        var length = Math.Min(startIndex + batchSize, sourceAsList.Length) - startIndex;
        
        var batch = new T[length];
        Array.Copy(sourceAsList, startIndex, batch, 0, length);
        yield return batch;
      }
    }

    /// <summary>
    /// Decorates an enumerable so that when it throws an exception of type <typeparamref name="TException"/> upon enumeration, a transformed 
    /// exception is thrown instead.
    /// </summary>
    public static IEnumerable<T> TransformExceptions<T, TException> (this IEnumerable<T> input, Func<TException, Exception> transformer)
        where TException : Exception
    {
      using (var enumerator = TransformStorageException(input.GetEnumerator, transformer))
      {
        while (TransformStorageException(enumerator.MoveNext, transformer))
        {
          yield return TransformStorageException(() => enumerator.Current, transformer);
        }
      }
    }

    private static T TransformStorageException<T, TException> ([InstantHandle] Func<T> func, Func<TException, Exception> transformer)
        where TException : Exception
    {
      try
      {
        return func();
      }
      catch (TException ex)
      {
        throw transformer(ex);
      }
    }
  }
}