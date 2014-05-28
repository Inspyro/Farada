using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.FastReflection
{
  public static class FastReflection
  {
    private static readonly ConcurrentDictionary<Type, IFastTypeInfo> s_typeInfos = new ConcurrentDictionary<Type, IFastTypeInfo>();

    private static readonly ConcurrentDictionary<PropertyInfo, IFastPropertyInfo> s_propertyInfos =
        new ConcurrentDictionary<PropertyInfo, IFastPropertyInfo>();

    public static IFastTypeInfo GetTypeInfo (Type type)
    {
      return s_typeInfos.GetOrAdd(type, CreateTypeInfo);
    }

    private static IFastTypeInfo CreateTypeInfo (Type type)
    {
      var fastProperties = type.GetProperties().Select(GetPropertyInfo).ToList();
      return new FastTypeInfo(fastProperties);
    }

    public static IFastPropertyInfo GetPropertyInfo (PropertyInfo propertyInfo)
    {
      return s_propertyInfos.GetOrAdd(propertyInfo, CreatePropertyInfo);
    }

    private static IFastPropertyInfo CreatePropertyInfo (PropertyInfo propertyInfo)
    {
      return new FastPropertyInfo(propertyInfo);
    }
  }
}
