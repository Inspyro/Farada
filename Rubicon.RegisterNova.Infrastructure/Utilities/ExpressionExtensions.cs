using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider.Keys;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.Utilities
{
  internal static class ExpressionExtensions
  {
    public static IEnumerable<PropertyKeyPart> ToChain(this Expression expression)
    {
      var memberExpression = expression as MemberExpression;
      if (memberExpression != null)
      {
        foreach (var chainKey in memberExpression.Expression.ToChain())
        {
          yield return chainKey;
        }

        yield return new PropertyKeyPart(FastReflection.GetPropertyInfo(((PropertyInfo) memberExpression.Member)));
      }
    }

    public static IEnumerable<PropertyKeyPart> ToChain (this LambdaExpression expression)
    {
      return expression.Body.ToChain();
    }

    public static Type GetParameterType(this Expression expression)
    {
      var parameterExpression = expression as ParameterExpression;

      if(parameterExpression != null)
        return parameterExpression.Type;

      var memberExpression = expression as MemberExpression;
      if (memberExpression != null)
      {
        return memberExpression.Expression.GetParameterType();
      }

      return null;
    }

    public static Type GetParameterType(this LambdaExpression expression)
    {
      return expression.Body.GetParameterType();
    }
  }
}