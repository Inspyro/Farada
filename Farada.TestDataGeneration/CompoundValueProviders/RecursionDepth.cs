using System;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// Helps to fill the recursion depth properties - more descriptive
  /// </summary>
  public static class RecursionDepth
  {
    /// <summary>
    /// Only instantiate the specified types, never fill any properties
    /// </summary>
    public const int DoNotFillProperties = 0;

    /// <summary>
    /// Fill all types on the first level, do not fill types in the hierarchy that where already filled on a higher level -> a.a is only instantiated a.a.a is null
    /// </summary>
    public const int DoNotFillSecondLevelProperties = 1;

    /// <summary>
    /// Fill all types on the first and second level a.a is filled, a.a.a is instantiated a.a.a.a is null
    /// </summary>
    public const int Default = 2;
  }
}
