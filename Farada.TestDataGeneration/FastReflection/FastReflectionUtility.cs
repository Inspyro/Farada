using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Farada.TestDataGeneration.Extensions;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.FastReflection
{
  /// <summary>
  /// Provides a faster way to read properties and types than <see cref="System.Reflection"/>
  /// This class is thread safe
  /// </summary>
  public static class FastReflectionUtility
  {
    private static readonly ConcurrentDictionary<Type, IFastTypeInfo> s_typeInfos = new ConcurrentDictionary<Type, IFastTypeInfo>();

    /// <summary>
    /// Gets the complete type info from a type, with all properties already converted into <see cref="IFastMemberWithValues"/>
    /// If you want a single fast property use <see cref="CreatePropertyInfo"/>
    /// </summary>
    /// <param name="type">the type to reflect</param>
    /// <returns>the complete <see cref="IFastTypeInfo"/></returns>
    public static IFastTypeInfo GetTypeInfo (Type type)
    {
      return s_typeInfos.GetOrAdd(type, CreateTypeInfo);
    }

    private static IFastTypeInfo CreateTypeInfo (Type type)
    {
      if (type.IsCollection())
        return new FastTypeInfo (new IFastArgumentInfo[0], new IFastMemberWithValues[0]);

      //At the moment we only support the first constructor (immutability...)
      //as soon a specific ctor is needed - implement  a strategy for choosing the constructor
      var ctor = type.GetConstructors().FirstOrDefault();
      var fastCtorArguments = ctor != null ? ctor.GetParameters().Select (GetArgumentInfo).ToList() : new List<IFastArgumentInfo>();
      var fastProperties = type.GetProperties().Select(GetPropertyInfo).Concat(type.GetFields().Select(GetFieldInfo)).ToList();
      return new FastTypeInfo(fastCtorArguments, fastProperties);
    }

     /// <summary>
    /// Creates an <see cref="IFastArgumentInfo"/> for faster argument access
    /// </summary>
    /// <param name="parameterInfo">the parameter info to convert</param>
    /// <returns>The <see cref="IFastArgumentInfo"/></returns>
    public static IFastArgumentInfo GetArgumentInfo(ParameterInfo parameterInfo)
    {
      return CreateArgumentInfo (parameterInfo);
    }

    private static IFastArgumentInfo CreateArgumentInfo (ParameterInfo parameterInfo)
    {
       return new FastArgumentInfo(parameterInfo);
    }

    /// <summary>
    /// Creates an <see cref="IFastMemberWithValues"/> for faster property access
    /// </summary>
    /// <param name="propertyInfo">the property info to convert</param>
    /// <returns>The <see cref="IFastMemberWithValues"/></returns>
    public static IFastMemberWithValues GetPropertyInfo (PropertyInfo propertyInfo)
    {
      return CreatePropertyInfo(propertyInfo);
    }

    private static IFastMemberWithValues CreatePropertyInfo (PropertyInfo propertyInfo)
    {
      return new FastPropertyInfo(propertyInfo);
    }

    /// <summary>
    /// Creates an <see cref="IFastMemberWithValues"/> for faster property access
    /// </summary>
    /// <param name="fieldInfo">the field info to convert</param>
    /// <returns>The <see cref="IFastMemberWithValues"/></returns>
    public static IFastMemberWithValues GetFieldInfo (FieldInfo fieldInfo)
    {
      return CreateFieldInfo(fieldInfo);
    }

    private static IFastMemberWithValues CreateFieldInfo (FieldInfo fieldInfo)
    {
      return new FastFieldInfo(fieldInfo);
    }
  }
}
