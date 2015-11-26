using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.Extensions;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Provides a value for a specific type
  /// </summary>
  /// <typeparam name="TMember">The member type for which the value is created</typeparam>
  /// <typeparam name="TContext">The context under which the value is created</typeparam>
  public abstract class ValueProvider<TMember, TContext> : IValueProvider
      where TContext : ValueProviderContext<TMember>
  {
    object IValueProvider.Create (IValueProviderContext context)
    {
      return CreateValue((TContext) context);
    }

    IEnumerable<object> IValueProvider.CreateMany(IValueProviderContext context, int numberOfObjects)
    {
      return CreateManyValues ((TContext) context, numberOfObjects).Cast<object>();
    }

    public virtual bool CanHandle (Type memberType)
    {
      return memberType == typeof (TMember) || (memberType.IsNullableType() && memberType.GetTypeOfNullable() == typeof (TMember));
    }

    protected abstract TContext CreateContext (ValueProviderObjectContext objectContext);

    IValueProviderContext IValueProvider.CreateContext (ValueProviderObjectContext objectContext)
    {
      return CreateContext(objectContext);
    }

    /// <summary>
    /// Creates a value of the given property type
    /// </summary>
    /// <param name="context">the concrete context to considre</param>
    protected abstract TMember CreateValue (TContext context);

    protected virtual IEnumerable<TMember> CreateManyValues (TContext context, int numberOfObjects)
    {
      for (var i = 0; i < numberOfObjects; i++)
        yield return CreateValue(context);
    }
  }

  /// <summary>
  /// Like <see cref="ValueProvider{TMember,TContext}"/> but with the default context which is <see cref="ValueProviderContext{TProperty}"/>
  /// </summary>
  /// <typeparam name="TMember"></typeparam>
  public abstract class ValueProvider<TMember> : ValueProvider<TMember, ValueProviderContext<TMember>>
  {
    protected override ValueProviderContext<TMember> CreateContext (ValueProviderObjectContext objectContext)
    {
      return new ValueProviderContext<TMember>(objectContext);
    }
  }

}