using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;

namespace Farada.TestDataGeneration.Extensions
{
  /// <summary>
  /// Provides extensions for expressions which are only used internally for chain building etc...
  /// </summary>
  internal static class ExpressionExtensions
  {
    internal static IEnumerable<PropertyKeyPart> ToChain(this Expression expression)
    {
      var memberExpression = expression as MemberExpression;
      if (memberExpression != null)
      {
        foreach (var chainKey in memberExpression.Expression.ToChain())
        {
          yield return chainKey;
        }

        if (!(memberExpression.Member is PropertyInfo))
          throw new NotSupportedException (memberExpression.Member.Name + " is not a property. Members that are not properties are not supported");

        yield return new PropertyKeyPart(FastReflection.FastReflectionUtility.GetPropertyInfo(((PropertyInfo) memberExpression.Member)));
      }
    }

    internal static IEnumerable<PropertyKeyPart> ToChain (this LambdaExpression expression)
    {
      return expression.Body.ToChain();
    }

    internal static Type GetParameterType(this Expression expression)
    {
      var parameterExpression = expression as ParameterExpression;

      if(parameterExpression != null)
        return parameterExpression.Type;

      var memberExpression = expression as MemberExpression;
      if (memberExpression != null)
      {
        return memberExpression.Expression.GetParameterType();
      }

      throw new NotSupportedException ("A non parameter expression is not supported");
    }

    internal static Type GetParameterType(this LambdaExpression expression)
    {
      return expression.Body.GetParameterType();
    }
  }
}