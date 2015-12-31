using System;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.BaseDomain.Constraints
{
  /// <summary>
  /// A ValueProvider context, that includes <see cref="StringConstraints"/> which define the range of a string
  /// </summary>
  public class StringConstrainedValueProviderContext:ValueProviderContext<string>
  {
    /// <summary>
    /// The StringConstraints that should be considered by the value provider
    /// </summary>
    public StringConstraints StringConstraints { get; private set; }

    internal StringConstrainedValueProviderContext (ValueProviderObjectContext objectContext, StringConstraints stringConstraints)
        : base(objectContext)
    {
      StringConstraints = stringConstraints;
    }
  }
}