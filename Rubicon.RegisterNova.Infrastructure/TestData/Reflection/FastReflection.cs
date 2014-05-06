using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Rubicon.RegisterNova.Infrastructure.TestData.Reflection
{
  public static class FastReflection
  {
    private readonly static Dictionary<Type, IFastTypeInfo> s_typeInfos = new Dictionary<Type, IFastTypeInfo>();
    public static IFastTypeInfo GetTypeInfo(Type type)
    {
      if(s_typeInfos.ContainsKey(type))
      {
        return s_typeInfos[type];
      }

      var fastProperties = type.GetProperties().Select(propertyInfo=>(IFastPropertyInfo) new FastProperty(propertyInfo)).ToList();
      var fastTypeInfo = new FastTypeInfo(fastProperties);
      s_typeInfos.Add(type, fastTypeInfo);

      return fastTypeInfo;
    }
  }

  public interface IFastTypeInfo
  {
    IList<IFastPropertyInfo> Properties { get; }
  }

  public class FastTypeInfo:IFastTypeInfo
  {
    public IList<IFastPropertyInfo> Properties { get; private set; }

    public FastTypeInfo(IList<IFastPropertyInfo> properties )
    {
      Properties = properties;
    }
  }

  public interface IFastPropertyInfo
  {
    void SetValue (object instance, object value);
    string Name { get; }
    Type PropertyType { get; }
  }

  public class FastProperty:IFastPropertyInfo
  {
    private readonly Action<object, object> _setAction;
    public string Name { get; private set; }
    public Type PropertyType { get; private set; }

    public FastProperty (PropertyInfo propertyInfo)
    {
      var targetType = propertyInfo.DeclaringType;
      if(targetType==null)
      {
        throw new ArgumentException("PropertyInfo.DeclaringType was null");
      }

      var methodInfo = propertyInfo.GetSetMethod();

      var exTarget = Expression.Parameter(typeof(object), "t");
      var exValue = Expression.Parameter(typeof (object), "p");

      var exBody = Expression.Call(
          Expression.Convert(exTarget, targetType),
          methodInfo,
          new Expression[] { Expression.Convert(exValue, propertyInfo.PropertyType) });

      var lambda = Expression.Lambda<Action<object, object>>(exBody, exTarget, exValue);
      _setAction = lambda.Compile();

      Name = propertyInfo.Name;
      PropertyType = propertyInfo.PropertyType;
    }

    public void SetValue (object instance, object value)
    {
      _setAction(instance, value);
    }
  }
}
