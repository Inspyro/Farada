using System;
using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.ValueProviders
{
  public enum ValueFillMode
  {
    All,
    Specific,
    None
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
      return typeof (TMember).IsAssignableFrom(memberType);
    }

    public override bool FillsType (Type memberType)
    {
      switch (FillMode)
      {
        case ValueFillMode.All:
          return typeof (TMember).IsAssignableFrom (memberType);
        case ValueFillMode.Specific:
          return base.FillsType (memberType);
        case ValueFillMode.None:
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