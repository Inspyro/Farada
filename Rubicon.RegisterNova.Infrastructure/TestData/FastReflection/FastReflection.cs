using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.FastReflection
{
  public static class FastReflection
  {
    private readonly static Dictionary<Type, IFastTypeInfo> s_typeInfos = new Dictionary<Type, IFastTypeInfo>();
    public static IFastTypeInfo GetTypeInfo(Type type) //TODO: thread safe..
    {
      if(s_typeInfos.ContainsKey(type))
      {
        return s_typeInfos[type];
      }

      var fastProperties = type.GetProperties().Select(GetPropertyInfo).ToList();
      var fastTypeInfo = new FastTypeInfo(fastProperties);
      s_typeInfos.Add(type, fastTypeInfo);

      return fastTypeInfo;
    }

    public static IFastPropertyInfo GetPropertyInfo (PropertyInfo propertyInfo)
    {
      return new FastPropertyInfo(propertyInfo);
    }
  }
}
