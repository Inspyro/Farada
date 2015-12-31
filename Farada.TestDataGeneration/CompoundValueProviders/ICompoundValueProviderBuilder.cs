using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Modifiers;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// Builds a <see cref="CompoundValueProvider"/> based on the specified chains
  /// </summary>
  internal interface ICompoundValueProviderBuilder
  {
    /// <summary>
    /// Adds a generic object based value provider for a member in the chain
    /// </summary>
    /// <typeparam name="TContext">The type of the context for the value provider</typeparam>
    /// <param name="valueProvider">The value provider to inject in the chain</param>
    /// <param name="realType">The real type (reflected)</param>
    void AddProvider<TContext> (ValueProvider<object, TContext> valueProvider, Type realType) where TContext : ValueProviderContext<object>;


    /// <summary>
    /// Adds a provider for a member in the chain
    /// </summary>
    /// <typeparam name="TMember">The type of the member</typeparam>
    /// <typeparam name="TContext">The type of the context for the value provider</typeparam>
    /// <param name="valueProvider">The value provider to inject in the chain</param>
    void AddProvider<TMember, TContext> (ValueProvider<TMember, TContext> valueProvider) where TContext : ValueProviderContext<TMember>;


    /// <summary>
    /// Adds a provider for a member in the chain
    /// </summary>
    /// <typeparam name="TMember">The type of the member</typeparam>
    /// <typeparam name="TContainer">The type containing the member or the type itself</typeparam>
    /// <typeparam name="TContext">The type of the context for the value provider</typeparam>
    /// <param name="chainExpression">The expression that leads to the member (e.g. (Person p)=>p.Name) or to the type (e.g. (string s)=>s)</param>
    /// <param name="valueProvider">The value provider to inject in the chain</param>
    /// </param>
    void AddProvider<TMember, TContainer, TContext> (
        Expression<Func<TContainer, TMember>> chainExpression,
        ValueProvider<TMember, TContext> valueProvider) where TContext : ValueProviderContext<TMember>;

    /// <summary>
    /// Adds a provider for a member in the chain
    /// </summary>
    void AddProvider<TMember, TContainer, TMetadata, TContext>(
        Expression<Func<TContainer, TMember>> chainExpression,
        Func<BoundMetadataContext<TContainer>, TMetadata> metadataProviderFunc,
        ValueProvider<TMember, TContext> valueProvider) where TContext : ValueProviderContext<TMember>;

    /// <summary>
    /// Adds an <see cref="IInstanceModifier"/> that gets all instances after they are filled and can modifiy them
    /// </summary>
    /// <param name="instanceModifier">the instance modifier to inject</param>
    void AddInstanceModifier (IInstanceModifier instanceModifier);
  }
}