using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.Extensions
{
  /// <summary>
  /// Extensions methods for <see cref="Type"/>.
  /// </summary>
  public static class TypeExtensions
  {
    [CanBeNull] //TODO PRES-675: Check
    public static Type GetTypeOfNullable (this Type type)
    {
      if (!type.IsNullableType())
        throw new ArgumentException ("You cannot retrieve the nullable type of a type that is not nullable");

      return type.GetGenericArguments()[0];
    }

    public static bool IsNullableType (this Type type)
    {
      return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof (Nullable<>));
    }

    public static bool IsCollection (this Type type)
    {
      return type.GetInterface (typeof (IEnumerable).FullName) != null
             || type.GetInterface (typeof (IEnumerable<>).FullName) != null;
    }

    /// <summary>
    /// Checks if a type is derived from another type.
    /// </summary>
    public static bool IsDerivedFrom<T> (this Type type)
    {
      return typeof (T).IsAssignableFrom (type);
    }

    /// <summary>
    /// Checks if a type contains sub properties and is not a value type like string, int etc...
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <returns>true if the type is compound</returns>
    public static bool IsCompoundType (this Type type)
    {
      return !type.IsValueType && type.CanBeInstantiated();
    }

    /// <summary>
    /// Checks if the given type can be instantiated
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <param name="nonPublic">Wether to check for a non public (protected/private) way to create the type</param>
    /// <returns>true if the type can be instantiated</returns>
    public static bool CanBeInstantiated (this Type type, bool nonPublic = false)
    {
      return type.GetConstructor (
          nonPublic ? (BindingFlags.NonPublic | BindingFlags.Instance) : (BindingFlags.Public | BindingFlags.Instance),
          null,
          Type.EmptyTypes,
          null)
             != null;
    }

    /// <summary>
    /// Extracts the property info of a property, based on an expression
    /// </summary>
    /// <typeparam name="TSource">The type that contains the property</typeparam>
    /// <typeparam name="TResult">The type of the property itself</typeparam>
    /// <param name="propertyLambda">The expression that leads to the property</param>
    /// <returns>the property info</returns>
    /// <exception cref="ArgumentException">throws an exception if the expression does not refere to a property</exception>
    public static PropertyInfo GetPropertyInfo<TSource, TResult> (
        Expression<Func<TSource, TResult>> propertyLambda)
    {
      var unaryExpression = propertyLambda.Body as UnaryExpression;
      var operandExpression = unaryExpression != null ? unaryExpression.Operand : propertyLambda.Body;

      var member = operandExpression as MemberExpression;
      if (member == null)
      {
        throw new ArgumentException (string.Format ("Expression '{0}' refers to a method, not a property.", propertyLambda));
      }

      var propInfo = member.Member as PropertyInfo;
      if (propInfo == null)
      {
        throw new ArgumentException (string.Format ("Expression '{0}' refers to a field, not a property.", propertyLambda));
      }

      return propInfo;
    }
  }
}