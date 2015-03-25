using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.Extensions
{
  /// <summary>
  /// Provides Functionality for Equals methods
  /// </summary>
  public static class EqualityUtility
  {
    /// <summary>
    /// Checks wheter to classes match or not
    /// </summary>
    /// <typeparam name="T">The type of the classes</typeparam>
    /// <param name="this">The caller</param>
    /// <param name="other">The parameter of the equals method</param>
    /// <returns>true if the classes match</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ClassEquals<T>(T @this, [CanBeNull] T other)
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
