using System;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.Extensions;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates random ulongs
  /// </summary>
  public class RandomULongGenerator:RangeConstrainedValueProvider<ulong>
  {
    protected override ulong DefaultMinValue
    {
      get { return ulong.MinValue; }
    }

    protected override ulong DefaultMaxValue
    {
      get { return uint.MaxValue; }
    }

    protected override ulong CreateValue (RangeConstrainedValueProviderContext<ulong> context)
    {
      return context.Random.Next(context.RangeContstraints.MinValue, context.RangeContstraints.MaxValue);
    }
  }
}