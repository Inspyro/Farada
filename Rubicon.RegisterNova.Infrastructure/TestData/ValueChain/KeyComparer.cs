using System;
using System.Collections.Generic;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  internal class KeyComparer : IEqualityComparer<Key>
  {
    public bool Equals (Key x, Key y)
    {
      return x.Equals(y);
    }

    public int GetHashCode (Key obj)
    {
      return obj.GetHashCode();
    }
  }
}