using System;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.Extensions;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random uint
  /// </summary>
  public class RandomUIntGenerator:RangeConstrainedValueProvider<uint>
  {
    protected override uint DefaultMinValue
    {
      get { return uint.MinValue; }
    }

    protected override uint DefaultMaxValue
    {
      get { return uint.MaxValue; }
    }

    protected override uint CreateValue (RangeConstrainedValueProviderContext<uint> context)
    {
      return context.Random.Next(context.RangeContstraints.MinValue, context.RangeContstraints.MaxValue);
    }
  }
}