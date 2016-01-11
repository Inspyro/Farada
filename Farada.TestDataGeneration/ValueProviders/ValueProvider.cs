using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders.Context;
using JetBrains.Annotations;

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
    IEnumerable<object> IValueProvider.CreateMany (IValueProviderContext context, [CanBeNull] IList<object> metadatas, int itemCount)
    {
      return CreateManyValues ((TContext) context, metadatas, itemCount).Cast<object>();
    }

    public virtual bool CanHandle (Type memberType)
    {
      return memberType==typeof(TMember) || memberType.UnwrapIfNullable() == typeof (TMember);
    }

    public virtual bool FillsType (Type memberType)
    {
      //by default all value provider should be able to fill the direct type they were registered for.
      //e.g if you registered a value provider for Vehicle, it should be possible to fill the vehicle type completely 
      //however the user can always enforce auto fill by using EnableAutoFill() in the registration.
      return memberType == typeof(TMember) || memberType.UnwrapIfNullable() == typeof (TMember);
    }

    protected abstract TContext CreateContext (ValueProviderObjectContext objectContext);

    IValueProviderContext IValueProvider.CreateContext (ValueProviderObjectContext objectContext)
    {
      return CreateContext (objectContext);
    }

    /// <summary>
    /// Creates a value of the given property type
    /// </summary>
    /// <param name="context">the concrete context to consider.</param>
    [CanBeNull]
    protected abstract TMember CreateValue (TContext context);

    protected virtual IEnumerable<TMember> CreateManyValues (TContext context, [CanBeNull] IList<object> metadatas, int itemCount)
    {
      for (var i = 0; i < itemCount; i++)
        yield return CreateValue (metadatas == null ? context : context.Enrich<TContext> (metadatas[i]));
    }
  }

  public abstract class ValueProvider<TMember> : ValueProvider<TMember, ValueProviderContext<TMember>>
  {
    protected override ValueProviderContext<TMember> CreateContext (ValueProviderObjectContext objectContext)
    {
      return new ValueProviderContext<TMember> (objectContext);
    }
  }
}