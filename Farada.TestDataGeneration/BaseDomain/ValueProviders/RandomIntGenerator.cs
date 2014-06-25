using System;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random integer reading the <see cref="RangeContstraints{T}"/> from the property
  /// </summary>
  public class RandomIntGenerator:ValueProvider<int, RangeConstrainedValueProviderContext<int>>
  {
    protected override RangeConstrainedValueProviderContext<int> CreateContext (ValueProviderObjectContext objectContext)
    {
      var rangeContstraints = RangeContstraints<int>.FromProperty(objectContext.PropertyInfo)
                              ?? new RangeContstraints<int>(int.MinValue, int.MaxValue);

      return new RangeConstrainedValueProviderContext<int>(objectContext, rangeContstraints);
    }

    protected override int CreateValue (RangeConstrainedValueProviderContext<int> context)
    {
      return context.Random.Next(context.RangeContstraints.MinValue, context.RangeContstraints.MaxValue);
    }
  }

 
}
