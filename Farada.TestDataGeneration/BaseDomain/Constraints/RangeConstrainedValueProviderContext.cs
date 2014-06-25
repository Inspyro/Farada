using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.Constraints
{
  /// <summary>
  /// A ValueProvider context, that includes <see cref="RangeContstraints"/> which usually define the range of a number (int, double,...)
  /// </summary>
  /// <typeparam name="T">The type of the context which also declares the type of the range constraints</typeparam>
  public class RangeConstrainedValueProviderContext<T>:ValueProviderContext<T>
      where T : IComparable
  {
    /// <summary>
    /// The RangeConstraints that should be considered by the value provider
    /// </summary>
    public RangeContstraints<T> RangeContstraints { get; private set; }

    internal RangeConstrainedValueProviderContext (ValueProviderObjectContext objectContext, RangeContstraints<T> rangeContstraints)
        : base(objectContext)
    {
      RangeContstraints = rangeContstraints;
    }
  }
}