using System;
using System.Collections.Generic;
using System.Linq;

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

      var fastProperties = type.GetProperties().Select(propertyInfo=>(IFastPropertyInfo) new FastPropertyInfo(propertyInfo)).ToList();
      var fastTypeInfo = new FastTypeInfo(fastProperties);
      s_typeInfos.Add(type, fastTypeInfo);

      return fastTypeInfo;
    }
  }
}
