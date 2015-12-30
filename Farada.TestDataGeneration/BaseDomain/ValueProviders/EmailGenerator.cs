using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a value that respects the email address attribute
  /// </summary>
  public class EmailGenerator: AttributeBasedValueProvider<string, EmailAddressAttribute> //TODO: test and implement
  {
    protected override string CreateAttributeBasedValue(AttributeValueProviderContext<string, EmailAddressAttribute> context)
    {
      return "some@gmx.at";
    }
  }
}
