using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.ValueProviders;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// A Value provider which selects random values from a given input list.
  /// </summary>
  public class ChooseSingleItemValueProvider<TSource, TResult> : ValueProvider<TResult>
  {
    private readonly IReadOnlyList<TSource> _items;
    private readonly Func<TSource, TResult> _conversionFunc;

    public ChooseSingleItemValueProvider (
        IReadOnlyList<TSource> items,
        Func<TSource, TResult> conversionFunc)
    {
      _items = items;
      _conversionFunc = conversionFunc;
    }

    protected override TResult CreateValue ([NotNull] ValueProviderContext<TResult> context)
    {
      var item = _items[context.Random.Next(0, _items.Count)];
      return _conversionFunc(item);
    }
  }
}