using System;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// A value provider with additional custom data.
  /// </summary>
  /// <typeparam name="TMember"></typeparam>
  /// <typeparam name="TAdditionalData"></typeparam>
  public class ExtendedValueProviderContext<TMember, TAdditionalData> : ValueProviderContext<TMember>
  {
    public TAdditionalData AdditionalData { get; private set; }

    public ExtendedValueProviderContext ([NotNull] ValueProviderObjectContext objectContext, TAdditionalData additionalData)
        : base (objectContext)
    {
      AdditionalData = additionalData;
    }
  }
}