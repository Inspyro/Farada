using System.Collections.Generic;
using Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  public interface IMetadataResolver
  {
    bool NeedsMetadata (IKey memberKey);
    
    IEnumerable<object> Resolve (IKey memberKey, IList<MetadataObjectContext> metadataContexts);
  }
}