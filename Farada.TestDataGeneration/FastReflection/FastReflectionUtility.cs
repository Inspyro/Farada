using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
      //At the moment we only support the first constructor (immutability...)
      //as soon a specific ctor is needed - implement  a strategy for choosing the constructor
      var ctor = type.GetConstructors().FirstOrDefault();
      var fastCtorArguments = ctor != null ? ctor.GetParameters().Select (GetArgumentInfo).ToList() : new List<IFastArgumentInfo>();
      var fastProperties = type.GetProperties().Select(GetPropertyInfo).ToList();
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
