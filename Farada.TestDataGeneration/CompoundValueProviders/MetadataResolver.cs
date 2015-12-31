using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// Resolves properties from intances based on the member information in the provided keys.
  /// </summary>
  internal class MetadataResolver:IMetadataResolver
  {
    private readonly Dictionary<IKey, Func<MetadataObjectContext, object>> _metadataProviderMapping;

    public MetadataResolver(Dictionary<IKey, Func<MetadataObjectContext, object>> metadataProviderMapping)
    {
      _metadataProviderMapping = metadataProviderMapping;
    }

    public bool NeedsMetadata(IKey memberKey)
    {
      if (memberKey.Member == null)
        throw new InvalidOperationException("Could not resolve metadata for member " + memberKey + " because member = null. TODO");

      return _metadataProviderMapping.ContainsKey(memberKey);
    }
    
    public IEnumerable<object> Resolve (IKey memberKey, IList<MetadataObjectContext> metadataContexts)
    {
      if (!NeedsMetadata (memberKey))
        throw new InvalidOperationException ("This member does not need metadata. Check with NeedsMetadata method before.");

      if (metadataContexts.Count <= 0)
      {
        throw new ArgumentException (
            $"Could not find metadata context for key:'{memberKey}'. Have you registered the dependency before the metadata provider?");
      }

      var metadataProvider = _metadataProviderMapping[memberKey];
      return metadataContexts.Select (metadataContext => metadataProvider (metadataContext));
    }
  }
}