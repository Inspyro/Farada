﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Farada.TestDataGeneration.FastReflection
{
  /// <summary>
  /// Provides a faster way to read properties and types than <see cref="System.Reflection"/>
  /// This class is thread safe
  /// </summary>
  public static class FastReflection
  {
    private static readonly ConcurrentDictionary<Type, IFastTypeInfo> s_typeInfos = new ConcurrentDictionary<Type, IFastTypeInfo>();

    /// <summary>
    /// Gets the complete type info from a type, with all properties already converted into <see cref="IFastPropertyInfo"/>
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
      var fastProperties = type.GetProperties().Select(GetPropertyInfo).ToList();
      return new FastTypeInfo(fastProperties);
    }

    /// <summary>
    /// Creates an <see cref="IFastPropertyInfo"/> for faster property access
    /// </summary>
    /// <param name="propertyInfo">the property info to convert</param>
    /// <returns>The <see cref="IFastPropertyInfo"/></returns>
    public static IFastPropertyInfo GetPropertyInfo (PropertyInfo propertyInfo)
    {
      return CreatePropertyInfo(propertyInfo);
    }

    private static IFastPropertyInfo CreatePropertyInfo (PropertyInfo propertyInfo)
    {
      return new FastPropertyInfo(propertyInfo);
    }
  }
}