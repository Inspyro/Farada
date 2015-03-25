using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Farada.TestDataGeneration.Extensions;

namespace Farada.TestDataGeneration.FastReflection
{
  /// <summary>
  /// Provides a faster way to create a class then the standard <see cref="Activator"/>
  /// </summary>
  public static class FastActivator
  {
    private static readonly ConcurrentDictionary<Type, Func<object>> s_creatorCache = new ConcurrentDictionary<Type, Func<object>>();

    private static readonly ConcurrentDictionary<Type, Func<object[], object>> s_creatorCache2 =
        new ConcurrentDictionary<Type, Func<object[], object>>();

    ///<summary>
    /// Create an object that will used as a 'factory' to the specified type T  
    /// </summary>
    /// <returns>the factory the creates a new object of type T</returns>
    public static Func<object> GetFactory (Type t)
    {
      return s_creatorCache.GetOrAdd (t, CreateFactoryMethod);
    }

    public static Func<object[], object> GetFactory (Type type, IList<IFastArgumentInfo> ctorArguments)
    {
      return s_creatorCache2.GetOrAdd (type, t => CreateFactoryMethod (t, ctorArguments));
    }

    private static Func<object> CreateFactoryMethod (Type type)
    {
      if (type.IsValueType)
        throw new ArgumentException ("Value types cannot be instantiated");

      if (!type.CanBeInstantiated())
      {
        if (type.CanBeInstantiated (nonPublic: true))
        {
          throw new NotSupportedException ("Classes with non-public constructors are not supported");
        }

        throw new NotSupportedException ("Classes without default constructors are not supported");
      }

      var expression = Expression.Lambda<Func<object>> (Expression.New (type));
      return expression.Compile();
    }


    private static Func<object[], object> CreateFactoryMethod (Type type, IList<IFastArgumentInfo> ctorArguments)
    {
      var constructor = type.GetConstructor (
          BindingFlags.Instance | BindingFlags.Public,
          null,
          CallingConventions.HasThis,
          ctorArguments.Select (argument => argument.Type).ToArray(),
          new ParameterModifier[0]);

      var argsParam = Expression.Parameter (typeof (object[]), "args");

      var constructorParameterExpressions =
          ctorArguments.Select ((argument, i) => Expression.Convert (Expression.ArrayIndex (argsParam, Expression.Constant (i)), argument.Type));

      var expression = Expression.Lambda<Func<object[], object>> (
          Expression.New (constructor, constructorParameterExpressions),
          argsParam);

      return expression.Compile();
    }
  }
}