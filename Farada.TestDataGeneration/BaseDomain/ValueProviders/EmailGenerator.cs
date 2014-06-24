using System;
using System.ComponentModel.DataAnnotations;
using Farada.TestDataGeneration.ValueProvider;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  public class EmailGenerator:AttributeBasedValueProvider<string, EmailAddressAttribute>
  {
    protected override string CreateValue (AttributeValueProviderContext<string, EmailAddressAttribute> context)
    {
      return "some@gmx.at";
    }
  }
}
