using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.FastReflection
{
  /// <summary>
  /// Provides a faster way to access a member than <see cref="MemberInfo"/>
  /// </summary>
  public interface IFastMemberInfo
  {
    /// <summary>
    /// A fast way to get an attribute from the member
    /// </summary>
    /// <typeparam name="T">the type of the attribute</typeparam>
    /// <returns>the attribute instance from the member</returns>
    [CanBeNull]
    T GetCustomAttribute<T> () where T : Attribute;

    /// <summary>
    /// The attribute types that are on the member
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
    /// The name of the member <see cref="MemberInfo.Name"/>
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The type of the member <see cref="MemberInfo.MemberType"/>
    /// </summary>
    Type Type { get; }
  }
}