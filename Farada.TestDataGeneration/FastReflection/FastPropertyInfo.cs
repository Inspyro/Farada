using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Farada.TestDataGeneration.FastReflection
{
  public class FastPropertyInfo:IFastPropertyInfo
  {
    private readonly Func<object, object> _getFunction;
    private readonly Action<object, object> _setAction;
    private readonly List<Type> _attributeTypes;
    private readonly List<Attribute> _attributes;

    public string Name { get; private set; }
    public Type PropertyType { get; private set; }

    internal FastPropertyInfo (PropertyInfo propertyInfo)
    {
      var targetType = propertyInfo.DeclaringType;
      if(targetType==null)
      {
        throw new ArgumentException("PropertyInfo.DeclaringType was null");
      }

      Name = propertyInfo.Name;
      PropertyType = propertyInfo.PropertyType;

      _attributes = propertyInfo.GetCustomAttributes().ToList();
      _attributeTypes = _attributes.Select(a=>a.GetType()).ToList();

       _getFunction = CreateGetFunction(propertyInfo, targetType);
      _setAction=CreateSetAction(propertyInfo, targetType);
    }

    private static Func<object, object> CreateGetFunction (PropertyInfo propertyInfo, Type targetType)
    {
      var methodInfo = propertyInfo.GetGetMethod();
      var exTarget = Expression.Parameter(typeof (object), "t");

      var exBody = Expression.Convert(Expression.Call(Expression.Convert(exTarget, targetType), methodInfo), typeof (object));
      var lambda = Expression.Lambda<Func<object, object>>(exBody, exTarget);

      return lambda.Compile();
    }

    private static Action<object, object> CreateSetAction (PropertyInfo propertyInfo, Type targetType)
    {
      var methodInfo = propertyInfo.GetSetMethod();

      if (methodInfo == null)
        return (o, o1) => { };

      var exTarget = Expression.Parameter(typeof (object), "t");
      var exValue = Expression.Parameter(typeof (object), "p");

      var exBody = Expression.Call(
          Expression.Convert(exTarget, targetType),
          methodInfo,
          new Expression[] { Expression.Convert(exValue, propertyInfo.PropertyType) });

      var lambda = Expression.Lambda<Action<object, object>>(exBody, exTarget, exValue);
      return lambda.Compile();
    }

    public object GetValue (object instance)
    {
      return _getFunction(instance);
    }

    public void SetValue (object instance, object value)
    {
      _setAction(instance, value);
    }

    public T GetCustomAttribute<T> () where T : Attribute
    {
      return (T) _attributes.FirstOrDefault(a => a is T); 
    }

    public IEnumerable<Type> Attributes
    {
      get { return _attributeTypes; }
    }

    public bool IsDefined (Type type)
    {
      return _attributeTypes.Any(type.IsAssignableFrom);
    }
  }

  public interface IFastPropertyInfo
  {
    T GetCustomAttribute<T> () where T : Attribute;
    IEnumerable<Type> Attributes { get; }

    bool IsDefined (Type type);
    object GetValue (object instance);
    void SetValue (object instance, object value);
    string Name { get; }
    Type PropertyType { get; }
  }
}