using System;
using System.Collections.Generic;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  internal class ChainKeyComparer : IEqualityComparer<ChainKey>
  {
    public bool Equals (ChainKey x, ChainKey y)
    {
      return x.Equals(y);
    }

    public int GetHashCode (ChainKey obj)
    {
      return obj.GetHashCode();
    }
  }
}