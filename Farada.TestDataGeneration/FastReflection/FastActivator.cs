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
    private static readonly ConcurrentDictionary<Type, Func<object[], object>> s_creatorCache =
        new ConcurrentDictionary<Type, Func<object[], object>>();

    ///<summary>
    /// Create an object that will used as a 'factory' to the specified type T  
    /// </summary>
    /// <returns>the factory the creates a new object of type T</returns>
    public static Func<object[], object> GetFactory (Type type, IList<IFastArgumentInfo> ctorArguments)
    {
      return s_creatorCache.GetOrAdd (type, t => CreateFactoryMethod (t, ctorArguments));
    }

    private static Func<object[], object> CreateFactoryMethod (Type type, IList<IFastArgumentInfo> ctorArguments)
    {
      if (type.IsPrimitive)
        throw new NotSupportedException ("Primitive types are not supported. Please configure a value provider for type " + type + ".");

      if (type.IsArray)
        throw new NotSupportedException ("Array types are not supported. Please configure a value provider");


      //sample: (params) => new Ice(params[0], params[1]);
      var constructor = type.GetConstructor (
          BindingFlags.Instance | BindingFlags.Public,
          null,
          CallingConventions.HasThis,
          ctorArguments.Select (argument => argument.Type).ToArray(),
          new ParameterModifier[0]);

      if (constructor == null)
        throw new NotSupportedException (
            "No valid ctor found on '"+type+"': Classes with non-public constructors and abstract classes are not supported.");

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