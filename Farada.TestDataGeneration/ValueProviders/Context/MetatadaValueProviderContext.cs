using System;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.ValueProviders.Context
{
  /// <summary>
  /// A value provider context with metadata.
  /// </summary>
  /// <typeparam name="TMember"></typeparam>
  /// <typeparam name="TMetadata"></typeparam>
  public class MetatadaValueProviderContext<TMember, TMetadata> : ValueProviderContext<TMember>
  {
    public TMetadata Metadata { get; private set; }

    public MetatadaValueProviderContext ([NotNull] ValueProviderObjectContext objectContext)
        : base (objectContext)
    {
    }

    internal override TContext Enrich<TContext> (object metadata)
    {
      Metadata = (TMetadata) metadata; //REVIEW: Should we catch the potential cast exception here?
      return base.Enrich<TContext> (metadata);
    }
  }
}