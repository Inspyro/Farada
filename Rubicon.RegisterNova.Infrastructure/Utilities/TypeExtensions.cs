using System;
using System.Reflection;

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

    public static bool IsCompoundType (this Type type)
    {
      return !type.IsValueType && type.CanBeInstantiated();
    }

    public static bool CanBeInstantiated(this Type type, bool nonPublic=false)
    {
      return type.GetConstructor(nonPublic ? (BindingFlags.NonPublic|BindingFlags.Instance) : (BindingFlags.Public | BindingFlags.Instance), null, Type.EmptyTypes, null)
             != null;
    }
  }
}