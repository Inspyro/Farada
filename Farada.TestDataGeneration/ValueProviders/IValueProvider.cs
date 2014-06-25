using System;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Provides a value for a given <see cref="IValueProviderContext"/>
  /// </summary>
  internal interface IValueProvider
  {
    /// <summary>
    /// Creates an object value considering the given <see cref="IValueProviderContext"/>
    /// </summary>
    /// <param name="context">the context for the value generation</param>
    /// <returns>the created value</returns>
    object CreateValue (IValueProviderContext context);

    /// <summary>
    /// Checks wether the given type can be handled by this ValueProvider
    /// </summary>
    /// <param name="propertyType">the type that should be checked</param>
    /// <returns>true if the type can be handled/created</returns>
    bool CanHandle (Type propertyType);

    /// <summary>
    /// Creates the type safe context out of the non type safe context and might extend the context
    /// </summary>
    /// <param name="objectContext">the non type safe context, containing some basic info</param>
    /// <returns>the type safe and maybe extended context for this value provider</returns>
    IValueProviderContext CreateContext (ValueProviderObjectContext objectContext);
  }
}