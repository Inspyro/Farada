using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  internal interface IValueProvider
  {
    object CreateObjectValue (ValueProviderContext context);
  }
}