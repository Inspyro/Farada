using System;

namespace Farada.TestDataGeneration.ValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  internal interface IValueProvider
  {
    object CreateValue (IValueProviderContext context);
    bool CanHandle (Type propertyType);

    IValueProviderContext CreateContext (ValueProviderObjectContext objectContext);
  }
}