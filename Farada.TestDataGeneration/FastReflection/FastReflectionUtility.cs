using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Farada.TestDataGeneration.FastReflection
{
  public interface IFastReflectionUtility
  {
    /// <summary>
    /// Gets the complete type info from a type, with all properties already converted into <see cref="IFastMemberWithValues"/>
    /// If you want a single fast property use <see cref="FastReflectionUtility.CreatePropertyInfo"/>
    /// </summary>
    /// <param name="type">the type to reflect</param>
    /// <returns>the complete <see cref="IFastTypeInfo"/></returns>
    IFastTypeInfo GetTypeInfo (Type type);

    /// <summary>
    /// Creates an <see cref="IFastArgumentInfo"/> for faster argument access
    /// </summary>
    /// <param name="parameterInfo">the parameter info to convert</param>
    /// <returns>The <see cref="IFastArgumentInfo"/></returns>
    IFastArgumentInfo GetArgumentInfo(ParameterInfo parameterInfo);

    /// <summary>
    /// Creates an <see cref="IFastMemberWithValues"/> for faster property access
    /// </summary>
    /// <param name="propertyInfo">the property info to convert</param>
    /// <returns>The <see cref="IFastMemberWithValues"/></returns>
    IFastMemberWithValues GetPropertyInfo (PropertyInfo propertyInfo);

    /// <summary>
    /// Creates an <see cref="IFastMemberWithValues"/> for faster property access
    /// </summary>
    /// <param name="fieldInfo">the field info to convert</param>
    /// <returns>The <see cref="IFastMemberWithValues"/></returns>
    IFastMemberWithValues GetFieldInfo (FieldInfo fieldInfo);
  }

  /// <summary>
  /// Provides a faster way to read properties and types than <see cref="System.Reflection"/>
  /// This class is thread safe
  /// </summary>
  public class FastReflectionUtility : IFastReflectionUtility
  {
    private readonly ConcurrentDictionary<Type, IFastTypeInfo> _typeInfos = new ConcurrentDictionary<Type, IFastTypeInfo>();

    /// <summary>
    /// Gets the complete type info from a type, with all properties already converted into <see cref="IFastMemberWithValues"/>
    /// If you want a single fast property use <see cref="CreatePropertyInfo"/>
    /// </summary>
    /// <param name="type">the type to reflect</param>
    /// <returns>the complete <see cref="IFastTypeInfo"/></returns>
    public IFastTypeInfo GetTypeInfo (Type type)
    {
      return _typeInfos.GetOrAdd(type, CreateTypeInfo);
    }

    private IFastTypeInfo CreateTypeInfo (Type type)
    {
      //we choose the ctor with the lowest parameter count (easiest to construct).
      var ctor = type.GetConstructors().OrderBy (c => c.GetParameters().Length).FirstOrDefault();
      var fastCtorArguments = ctor?.GetParameters().Select (GetArgumentInfo).ToList() ?? new List<IFastArgumentInfo>();

      //we filter indexers and readonly properties...
      var properties =
          type.GetProperties (BindingFlags.Instance | BindingFlags.Public | BindingFlags.Public)
              .Where (p => p.SetMethod != null)
              .Where (p => p.GetIndexParameters().Length == 0);

      var fields =
          type.GetFields (BindingFlags.Instance | BindingFlags.Public | BindingFlags.Public)
              .Where (f => !f.IsInitOnly)
              .Where (f => !f.IsLiteral);

      var fastProperties = properties.Select (GetPropertyInfo).Concat (fields.Select (GetFieldInfo)).ToList();
      return new FastTypeInfo (fastCtorArguments, fastProperties);
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
       return new FastArgumentInfo(parameterInfo);
    }

    /// <summary>
    /// Creates an <see cref="IFastMemberWithValues"/> for faster property access
    /// </summary>
    /// <param name="propertyInfo">the property info to convert</param>
    /// <returns>The <see cref="IFastMemberWithValues"/></returns>
    public IFastMemberWithValues GetPropertyInfo (PropertyInfo propertyInfo)
    {
      return CreatePropertyInfo(propertyInfo);
    }

    private IFastMemberWithValues CreatePropertyInfo (PropertyInfo propertyInfo)
    {
      return new FastPropertyInfo(propertyInfo);
    }

    /// <summary>
    /// Creates an <see cref="IFastMemberWithValues"/> for faster property access
    /// </summary>
    /// <param name="fieldInfo">the field info to convert</param>
    /// <returns>The <see cref="IFastMemberWithValues"/></returns>
    public IFastMemberWithValues GetFieldInfo (FieldInfo fieldInfo)
    {
      return CreateFieldInfo(fieldInfo);
    }

    private IFastMemberWithValues CreateFieldInfo (FieldInfo fieldInfo)
    {
      return new FastFieldInfo(fieldInfo);
    }
  }
}
