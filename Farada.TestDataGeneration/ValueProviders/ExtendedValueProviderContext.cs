using System;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// A value provider with additional custom data.
  /// </summary>
  /// <typeparam name="TContainer"></typeparam>
  /// <typeparam name="TMember"></typeparam>
  /// <typeparam name="TAdditionalData"></typeparam>
  public class ExtendedValueProviderContext<TContainer, TMember, TAdditionalData> : ValueProviderContext<TContainer, TMember>
  {
    public TAdditionalData AdditionalData { get; private set; }

    public ExtendedValueProviderContext ([NotNull] ValueProviderObjectContext objectContext, TAdditionalData additionalData)
        : base (objectContext)
    {
      AdditionalData = additionalData;
    }
  }

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