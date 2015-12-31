using System;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  public abstract class RangeConstrainedValueProvider<T>:ValueProvider<T, RangeConstrainedValueProviderContext<T>>
      where T : IComparable
  {
    protected override RangeConstrainedValueProviderContext<T> CreateContext (ValueProviderObjectContext objectContext)
    {
      var rangeContstraints = RangeContstraints<T>.FromMember(objectContext.Member)
                              ?? new RangeContstraints<T>(DefaultMinValue, DefaultMaxValue);

      return new RangeConstrainedValueProviderContext<T>(objectContext, rangeContstraints);
    }

    protected abstract T DefaultMinValue{get; }
    protected abstract T DefaultMaxValue { get; }
  }
}