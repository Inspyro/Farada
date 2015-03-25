using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.Extensions
{
  /// <summary>
  /// Provides extensions for expressions which are only used internally for chain building etc...
  /// </summary>
  internal static class ExpressionExtensions
  {
    internal static IEnumerable<PropertyKeyPart> ToChain(this Expression expression, FastReflectionUtility fastReflectionUtility)
    {
      var memberExpression = expression as MemberExpression;
      if (memberExpression != null)
      {
        foreach (var chainKey in memberExpression.Expression.ToChain(fastReflectionUtility))
        {
          yield return chainKey;
        }

        if (!(memberExpression.Member is PropertyInfo))
          throw new NotSupportedException (memberExpression.Member.Name + " is not a property. Members that are not properties are not supported");

        yield return new PropertyKeyPart(fastReflectionUtility.GetPropertyInfo(((PropertyInfo) memberExpression.Member)));
      }
    }

    internal static IEnumerable<PropertyKeyPart> ToChain (this LambdaExpression expression, FastReflectionUtility fastReflectionUtility)
    {
      return expression.Body.ToChain(fastReflectionUtility);
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