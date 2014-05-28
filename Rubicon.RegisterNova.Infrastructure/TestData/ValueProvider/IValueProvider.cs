using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
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