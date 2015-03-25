using System;
using System.Collections.Generic;
using System.Reflection;

namespace Farada.TestDataGeneration.FastReflection
{
  /// <summary>
  /// Provides a faster way to access a property than <see cref="PropertyInfo"/>
  /// </summary>
  public interface IFastParameterInfo
  {
    /// <summary>
    /// A fast way to get an attribute from the property
    /// </summary>
    /// <typeparam name="T">the type of the attribute</typeparam>
    /// <returns>the attribute instance from the property</returns>
    T GetCustomAttribute<T> () where T : Attribute;

    /// <summary>
    /// The attribute types that are on the property
    /// To check if an attribute is on the type you can also use <see cref="IsDefined"/>
    /// </summary>
    IEnumerable<Type> Attributes { get; }

    /// <summary>
    /// Checks if the given attribute is defined on the type (you can also check <see cref="Attributes"/>
    /// </summary>
    /// <param name="type">the type or base type of the attribute</param>
    /// <returns>true if the attribute was found</returns>
    bool IsDefined (Type type);

    /// <summary>
    /// The name of the property <see cref="MemberInfo.Name"/>
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The type of the property <see cref="PropertyInfo.PropertyType"/>
    /// </summary>
    Type Type { get; }
  }
}