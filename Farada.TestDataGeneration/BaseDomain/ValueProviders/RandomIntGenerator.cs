using System;
using Farada.TestDataGeneration.BaseDomain.Constraints;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random integer reading the <see cref="RangeContstraints{T}"/> from the member
  /// </summary>
  public class RandomIntGenerator:RangeConstrainedValueProvider<int>
  {
    protected override int DefaultMinValue
    {
      get { return int.MinValue; }
    }

    protected override int DefaultMaxValue
    {
      get { return int.MaxValue; }
    }

    protected override int CreateValue (RangeConstrainedValueProviderContext<int> context)
    {
      return context.Random.Next(context.RangeContstraints.MinValue, context.RangeContstraints.MaxValue);
    }
  }
}
