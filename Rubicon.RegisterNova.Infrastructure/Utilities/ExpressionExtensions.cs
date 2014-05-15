﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Rubicon.RegisterNova.Infrastructure.TestData;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.Utilities
{
  internal static class ExpressionExtensions
  {
    //public static string GetName (this LambdaExpression expression)
    //{
    //  var memberExpression = expression.Body as MemberExpression;

    //  if (memberExpression == null)
    //  {
    //    throw new NotSupportedException();
    //  }

    //  var returnType = memberExpression.Type.FullName;
    //  var bodyInfo = memberExpression.Member.Name;
    //  var typeInfo = memberExpression.Expression.Type;

    //  return string.Format("{0} {1}.{2}", returnType, typeInfo, bodyInfo);
    //}

    public static IEnumerable<KeyPart> ToChain(this Expression expression)
    {
      var parameterExpression = expression as ParameterExpression;
      if (parameterExpression != null)
      {
        yield return new KeyPart(parameterExpression.Type);
      }

      var memberExpression = expression as MemberExpression;
      if (memberExpression != null)
      {
        foreach (var chainKey in memberExpression.Expression.ToChain())
        {
          yield return chainKey;
        }

        yield return new KeyPart(memberExpression.Type, FastReflection.GetPropertyInfo(((PropertyInfo) memberExpression.Member)));
      }
    }

    public static IEnumerable<KeyPart> ToChain (this LambdaExpression expression)
    {
      return expression.Body.ToChain();
    }
  }
}