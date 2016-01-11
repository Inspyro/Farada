using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.ValueProviders
{
  public enum ValueFillMode
  {
    FillAll,
    FillSpecificInstance,
    FillNone
  }

  /// <summary>
  /// A <see cref="IValueProvider"/> that can not only generate properties of type TMember but also all subtypes
  /// </summary>
  /// <typeparam name="TMember">The type of the member</typeparam>
  /// <typeparam name="TContext">The type of the context for the member generation</typeparam>
  public abstract class SubTypeValueProvider<TMember, TContext> : ValueProvider<TMember, TContext>
      where TContext : ValueProviderContext<TMember>
  {
    public override bool CanHandle (Type memberType)
    {
      return typeof(TMember).IsAssignableFrom(memberType) || typeof (TMember).IsAssignableFrom(memberType.UnwrapIfNullable());
    }

    public override bool FillsType (Type memberType)
    {
      switch (FillMode)
      {
        case ValueFillMode.FillAll:
          return typeof (TMember).IsAssignableFrom (memberType) || typeof (TMember).IsAssignableFrom (memberType.UnwrapIfNullable());
        case ValueFillMode.FillSpecificInstance:
          return base.FillsType (memberType);
        case ValueFillMode.FillNone:
          return false;
      }

      throw new NotSupportedException (FillMode + " fill mode is not supported");
    }

    public abstract ValueFillMode FillMode { get; }
  }

  /// <summary>
  /// A <see cref="IValueProvider"/> that can not only generate properties of type TMember but also all subtypes
  /// like <see cref="SubTypeValueProvider{TMember,TContext}"/> but with the default context
  /// which is <see cref="ValueProviderContext{TMember}"/>
  /// </summary>
  /// <typeparam name="TMember">The type of the property</typeparam>
  public abstract class SubTypeValueProvider<TMember> : SubTypeValueProvider<TMember, ValueProviderContext<TMember>>
  {
    protected override ValueProviderContext<TMember> CreateContext(ValueProviderObjectContext objectContext)
    {
      return new ValueProviderContext<TMember>(objectContext);
    }
  }
}