using System;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random float
  /// </summary>
  public class RandomFloatGenerator:RangeConstrainedValueProvider<float> 
  {
    protected override float DefaultMinValue
    {
      get { return float.MinValue; }
    }

    protected override float DefaultMaxValue
    {
      get { return float.MaxValue; }
    }

    protected override float CreateValue (RangeConstrainedValueProviderContext<float> context)
    {
      return context.Random.Next(context.RangeContstraints.MinValue, context.RangeContstraints.MaxValue);
    }
  }
}