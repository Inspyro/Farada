using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
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
    internal static IEnumerable<MemberKeyPart> ToChain(this Expression expression)
    {
      var memberExpression = expression as MemberExpression;

      //DEBT: This is a hack to support convert statements (automatically introduced by the compiler).
      if(memberExpression == null)
      {
        var unaryExpression = expression as UnaryExpression;
        if(unaryExpression!=null)
          memberExpression=unaryExpression.Operand as MemberExpression;
      }

      if (memberExpression != null)
      {
        foreach (var chainKey in memberExpression.Expression.ToChain())
        {
          yield return chainKey;
        }

        var propertyInfo = memberExpression.Member as PropertyInfo;
        if (propertyInfo != null)
          yield return new MemberKeyPart(FastReflectionUtility.GetPropertyInfo(propertyInfo));

        var fieldInfo = memberExpression.Member as FieldInfo;
        if (fieldInfo != null)
          yield return new MemberKeyPart(FastReflectionUtility.GetFieldInfo(fieldInfo));

        if (propertyInfo == null && fieldInfo == null)
          throw new NotSupportedException(memberExpression.Member.Name + " is not a property or field, and thus not supported.");
      }
    }

    internal static IEnumerable<MemberKeyPart> ToChain (this LambdaExpression expression)
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