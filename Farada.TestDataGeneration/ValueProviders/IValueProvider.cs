using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Provides a value for a given <see cref="IValueProviderContext"/>
  /// </summary>
  internal interface IValueProvider
  {
    /// <summary>
    /// Creates many object values. See <see cref="Create"/> for details.
    /// </summary>
    /// <param name="context">the context for the value generation.</param>
    /// <param name="metadatas"></param>
    /// <param name="itemCount">The item count.</param>
    /// <returns>the created values</returns>
    IEnumerable<object> CreateMany(IValueProviderContext context, [CanBeNull] IList<object> metadatas, int itemCount);


    /// <summary>
    /// Checks wether the given type can be handled by this ValueProvider
    /// </summary>
    /// <param name="memberType">the type that should be checked</param>
    /// <returns>true if the type can be handled/created</returns>
    bool CanHandle (Type memberType);

    /// <summary>
    /// Checks wethere the given type will be filled by this ValueProvider or not. 
    /// </summary>
    /// <param name="type">the type that should be checked</param>
    /// <returns>true if the type will be filled by this value provider</returns>
    bool FillsType(Type type);

    /// <summary>
    /// Creates the type safe context out of the non type safe context and might extend the context
    /// </summary>
    /// <param name="objectContext">the non type safe context, containing some basic info</param>
    /// <returns>the type safe and maybe extended context for this value provider</returns>
    IValueProviderContext CreateContext (ValueProviderObjectContext objectContext);

  }
}