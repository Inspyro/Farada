using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.Modifiers;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// Builds a <see cref="CompoundValueProvider"/> based on the specified chains
  /// </summary>
  internal interface ICompoundValueProviderBuilder
  {

    /// <summary>
    /// Adds a provider for a property in the chain
    /// </summary>
    /// <typeparam name="TProperty">The type of the property</typeparam>
    /// <typeparam name="TContext">The type of the context for the value provider</typeparam>
    /// <param name="valueProvider">The value provider to inject in the chain</param>
    void AddProvider<TProperty, TContext> (ValueProvider<TProperty, TContext> valueProvider) where TContext : ValueProviderContext<TProperty>;

     /// <summary>
    /// Adds a provider for an attribute and a given return type
    /// You need to inject a provider for each attribute/type pair that you want to be filled
    /// </summary>
    /// <typeparam name="TProperty">The type of the properties which should filled</typeparam>
    /// <typeparam name="TAttribute">The type of the attribute that should be on the properties</typeparam>
    /// <typeparam name="TContext">The type of the context for the value provider</typeparam>
    /// <param name="attributeBasedValueProvider">the value provider to inject into the chain</param>
    void AddProvider<TProperty, TAttribute, TContext> (
        AttributeBasedValueProvider<TProperty, TAttribute, TContext> attributeBasedValueProvider) where TAttribute : Attribute where TContext : AttributeValueProviderContext<TProperty, TAttribute>;

    /// <summary>
    /// Adds a provider for a property in the chain
    /// </summary>
    /// <typeparam name="TProperty">The type of the property</typeparam>
    /// <typeparam name="TContainer">The type containing the property or the type itself</typeparam>
    /// <typeparam name="TContext">The type of the context for the value provider</typeparam>
    /// <param name="chainExpression">The expression that leads to the property (e.g. (Person p)=>p.Name) or to the type (e.g. (string s)=>s)</param>
    /// <param name="valueProvider">The value provider to inject in the chain</param>
    void AddProvider<TProperty, TContainer, TContext> (Expression<Func<TContainer, TProperty>> chainExpression, ValueProvider<TProperty, TContext> valueProvider) where TContext : ValueProviderContext<TProperty>;

    /// <summary>
    /// Adds an <see cref="IInstanceModifier"/> that gets all instances after they are filled and can modifiy them
    /// </summary>
    /// <param name="instanceModifier">the instance modifier to inject</param>
    void AddInstanceModifier (IInstanceModifier instanceModifier);
  }
}