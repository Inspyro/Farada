using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Farada.TestDataGeneration.FastReflection
{
  internal class FastPropertyInfo:IFastPropertyInfo
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

  /// <summary>
  /// Provides a faster way to access a property than <see cref="PropertyInfo"/>
  /// </summary>
  public interface IFastPropertyInfo
  {
    /// <summary>
    /// A fast way to get an attribute from the property
    /// </summary>
    /// <typeparam name="T">the type of the attribute</typeparam>
    /// <returns>the attribute instance from the property</returns>
    T GetCustomAttribute<T> () where T : Attribute;

     /// <summary>
    /// The attribute types that are on the property
    /// To check if an attribute is on the type you can also use <see cref="IsDefined"/>
    /// </summary>
    IEnumerable<Type> Attributes { get; }

    /// <summary>
    /// Checks if the given attribute is defined on the type (you can also check <see cref="Attributes"/>
    /// </summary>
    /// <param name="type">the type or base type of the attribute</param>
    /// <returns>true if the attribute was found</returns>
    bool IsDefined (Type type);

     /// <summary>
    /// A fast way to get the value of the property
    /// </summary>
    /// <param name="instance">the class instance</param>
    /// <returns>the value of the property</returns>
    object GetValue (object instance);

    /// <summary>
    /// A fast way to set the value of the property
    /// </summary>
    /// <param name="instance">the class instance</param>
    /// <param name="value">the value to set, needs to match the <see cref="PropertyType"/></param>
    void SetValue (object instance, object value);

     /// <summary>
    /// The name of the property <see cref="MemberInfo.Name"/>
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The type of the property <see cref="PropertyInfo.PropertyType"/>
    /// </summary>
    Type PropertyType { get; }
  }
}