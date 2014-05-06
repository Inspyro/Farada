using System;

namespace Rubicon.RegisterNova.Infrastructure.Utilities
{
  /// <summary>
  /// Extensions methods for <see cref="Type"/>.
  /// </summary>
  public static class TypeExtensions
  {
    /// <summary>
    /// Checks if a type is derived from another type.
    /// </summary>
    public static bool IsDerivedFrom<T> (this Type type)
    {
      return typeof(T).IsAssignableFrom(type);
    }

    public static bool CanBeInstantiated(this Type type)
    {
      return type.GetConstructor(Type.EmptyTypes) != null;
    }
  }
}