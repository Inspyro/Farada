using System.Collections.Generic;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  public interface IMemberSorter
  {
    IEnumerable<IFastMemberWithValues> Sort (IEnumerable<IFastMemberWithValues> members, IKey baseKey);
  }
}