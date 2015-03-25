using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Farada.TestDataGeneration.FastReflection
{
  /// <summary>
  /// Provides a faster way to read properties and types than <see cref="System.Reflection"/>
  /// This class is thread safe
  /// </summary>
  public class FastReflectionUtility
  {
    private readonly IParameterConversionService _parameterConversionService;
    private static readonly ConcurrentDictionary<Type, IFastTypeInfo> s_typeInfos = new ConcurrentDictionary<Type, IFastTypeInfo>();

    public FastReflectionUtility(IParameterConversionService parameterConversionService)
    {
      _parameterConversionService = parameterConversionService;
    }

    /// <summary>
    /// Gets the complete type info from a type, with all properties already converted into <see cref="IFastPropertyInfo"/>
    /// If you want a single fast property use <see cref="CreatePropertyInfo"/>
    /// </summary>
    /// <param name="type">the type to reflect</param>
    /// <returns>the complete <see cref="IFastTypeInfo"/></returns>
    public IFastTypeInfo GetTypeInfo (Type type)
    {
      return s_typeInfos.GetOrAdd(type, CreateTypeInfo);
    }

    private IFastTypeInfo CreateTypeInfo (Type type)
    {
      //At the moment we only support the first constructor (immutability...)
      //as soon a specific ctor is needed - implement  a strategy for choosing the constructor
      var fastCtorArguments = type.GetConstructors().First().GetParameters().Select (GetArgumentInfo).ToList();
      var fastProperties = type.GetProperties().Select(GetPropertyInfo).ToList();
      return new FastTypeInfo(fastCtorArguments, fastProperties);
    }

     /// <summary>
    /// Creates an <see cref="IFastArgumentInfo"/> for faster argument access
    /// </summary>
    /// <param name="parameterInfo">the parameter info to convert</param>
    /// <returns>The <see cref="IFastArgumentInfo"/></returns>
    public IFastArgumentInfo GetArgumentInfo(ParameterInfo parameterInfo)
    {
      return CreateArgumentInfo (parameterInfo);
    }

    private IFastArgumentInfo CreateArgumentInfo (ParameterInfo parameterInfo)
    {
       return new FastArgumentInfo(_parameterConversionService, parameterInfo);
    }

    /// <summary>
    /// Creates an <see cref="IFastPropertyInfo"/> for faster property access
    /// </summary>
    /// <param name="propertyInfo">the property info to convert</param>
    /// <returns>The <see cref="IFastPropertyInfo"/></returns>
    public IFastPropertyInfo GetPropertyInfo (PropertyInfo propertyInfo)
    {
      return CreatePropertyInfo(propertyInfo);
    }

    private IFastPropertyInfo CreatePropertyInfo (PropertyInfo propertyInfo)
    {
      return new FastPropertyInfo(propertyInfo);
    }
  }
}
