using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  internal class MemberContainerIndexSorter : IMemberSorter
  {
    private readonly Dictionary<IKey, int> _containerIndexMapping;

    public MemberContainerIndexSorter (Dictionary<IKey, int> containerIndexMapping)
    {
      _containerIndexMapping = containerIndexMapping;
    }

    public IEnumerable<IFastMemberWithValues> Sort (IEnumerable<IFastMemberWithValues> members, IKey baseKey)
    {
      return
          members.Select (m => new { Key = baseKey.CreateKey (m), Value = m })
              .OrderBy (keyValue => GetIndexForKey(keyValue.Key))
              .Select (keyValue => keyValue.Value);
    }

    private int GetIndexForKey (IKey key)
    {
      if (!_containerIndexMapping.ContainsKey (key))
        return int.MinValue; //means basically: a key that was never registered, can always be filled immediatly.

      return _containerIndexMapping[key];
    }
  }
}