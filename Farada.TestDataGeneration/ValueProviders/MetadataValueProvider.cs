using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.ValueProviders
{
  public abstract class MetadataValueProvider<TMember, TMetadata>
      : ValueProvider<TMember, MetatadaValueProviderContext<TMember, TMetadata>>
  {
    protected override MetatadaValueProviderContext<TMember, TMetadata> CreateContext (ValueProviderObjectContext objectContext)
    {
      return new MetatadaValueProviderContext<TMember, TMetadata> (objectContext);
    }
  }
}
