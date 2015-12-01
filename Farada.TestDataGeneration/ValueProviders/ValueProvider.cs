using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Extensions;
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
    IEnumerable<object> IValueProvider.CreateMany (
        IValueProviderContext context,
        [CanBeNull] IList<DependedPropertyCollection> dependedProperties,
        int itemCount)
    {
      return CreateManyValues ((TContext) context, dependedProperties, itemCount).Cast<object>();
    }

    public virtual bool CanHandle (Type memberType)
    {
      return memberType == typeof (TMember) || (memberType.IsNullableType() && memberType.GetTypeOfNullable() == typeof (TMember));
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
    protected abstract TMember CreateValue (TContext context);

    protected virtual IEnumerable<TMember> CreateManyValues (
        TContext context,
        [CanBeNull] IList<DependedPropertyCollection> dependedProperties,
        int itemCount)
    {
      for (var i = 0; i < itemCount; i++)
        yield return CreateValue (dependedProperties == null ? context : context.Enrich<TContext> (dependedProperties[i]));
    }
  }

  public abstract class ValueProviderWithContainer<TContainer, TMember> : ValueProvider<TMember, ValueProviderContext<TContainer, TMember>>
  {
    protected override ValueProviderContext<TContainer, TMember> CreateContext(ValueProviderObjectContext objectContext)
    {
      return new ValueProviderContext<TContainer, TMember>(objectContext);
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