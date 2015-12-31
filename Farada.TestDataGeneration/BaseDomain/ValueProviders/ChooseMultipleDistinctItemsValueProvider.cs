using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// A Value provider for <see cref="IList{T}"/> properties which uses an input list to produce random (unique) subsets.
  /// </summary>
  public class ChooseMultipleDistinctItemsValueProvider<TSource, TResult> : ValueProvider<IList<TResult>>
  {
    private readonly IReadOnlyList<TSource> _items;
    private readonly int _minItemCount;
    private readonly int _maxItemCount;
    private readonly Func<TSource, TResult> _conversionFunc;

    public ChooseMultipleDistinctItemsValueProvider (
        IReadOnlyList<TSource> items,
        int minItemCount,
        int maxItemCount,
        Func<TSource, TResult> conversionFunc)
    {
      if (minItemCount > maxItemCount)
        throw new ArgumentException("minItemCount must be lower or equal to maxItemCount.", "minItemCount");

      if (maxItemCount > items.Count)
        throw new ArgumentException("items list must have at least maxItemCount items.", "items");

      _items = items;
      _minItemCount = minItemCount;
      _maxItemCount = maxItemCount;
      _conversionFunc = conversionFunc;
    }

    protected override IList<TResult> CreateValue ([NotNull] ValueProviderContext<IList<TResult>> context)
    {
      var exclusiveMaxItemCount = _maxItemCount + 1;
      var numberOfReturnedItems = context.Random.Next(_minItemCount, exclusiveMaxItemCount);

      Trace.Assert(numberOfReturnedItems <= _items.Count);

      Func<int> randomIntGenerator =
        () => context.Random.Next(int.MinValue, int.MaxValue);

      return Enumerable.Range(0, _items.Count)
              .Shuffle(randomIntGenerator)
              .Take(numberOfReturnedItems)
              .Select(i => _items[i])
              .Select(_conversionFunc)
              .ToList();
    }
  }
}