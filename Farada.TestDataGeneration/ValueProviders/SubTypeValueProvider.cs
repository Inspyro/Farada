using System;

namespace Farada.TestDataGeneration.ValueProviders
{
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
  }

  /// <summary>
  /// A <see cref="IValueProvider"/> that can not only generate properties of type TMember but also all subtypes
  /// like <see cref="SubTypeValueProvider{TMember,TContext}"/> but with the default context
  /// which is <see cref="ValueProviderContext{TMember}"/>
  /// </summary>
  /// <typeparam name="TMember">The type of the property</typeparam>
  public abstract class SubTypeValueProvider<TMember> : ValueProvider<TMember>
  {
    public override bool CanHandle (Type memberType)
    {
      return typeof (TMember).IsAssignableFrom(memberType);
    }
  }
}