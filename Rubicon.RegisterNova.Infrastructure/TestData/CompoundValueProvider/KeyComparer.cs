using System;
using System.Collections.Generic;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  // REVIEW FS: Should we keep KeyComparer as well as this Equals()/GetHashCode() implementation?
  /// <summary>
  /// TODO
  /// </summary>
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