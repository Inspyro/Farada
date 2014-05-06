using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.Reflection
{
  public class FastPropertyInfo:IFastPropertyInfo
  {
    private readonly Action<object, object> _setAction;
    public string Name { get; private set; }
    public Type PropertyType { get; private set; }

    public FastPropertyInfo (PropertyInfo propertyInfo)
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

   public interface IFastPropertyInfo
  {
    void SetValue (object instance, object value);
    string Name { get; }
    Type PropertyType { get; }
  }
}