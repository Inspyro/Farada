using System;
using System.Runtime.CompilerServices;

namespace Farada.TestDataGeneration.Extensions
{
  public static class EqualityUtility
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ClassEquals<T>(T @this, T other)
    {
      if (ReferenceEquals(null, other))
        return false;

      if (ReferenceEquals(@this, other))
        return true;

      if (other.GetType() != @this.GetType())
        return false;

      return true;
    }
  }
}
