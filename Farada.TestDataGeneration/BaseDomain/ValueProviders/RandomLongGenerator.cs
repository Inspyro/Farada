using System;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.Extensions;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random long
  /// </summary>
  public class RandomLongGenerator:RangeConstrainedValueProvider<long>
  {
     protected override long DefaultMinValue
    {
      get { return long.MinValue; }
    }

    protected override long DefaultMaxValue
    {
      get { return long.MaxValue; }
    }

    protected override long CreateValue (RangeConstrainedValueProviderContext<long> context)
    {
      return context.Random.Next(context.RangeContstraints.MinValue, context.RangeContstraints.MaxValue);
    }
  }
}