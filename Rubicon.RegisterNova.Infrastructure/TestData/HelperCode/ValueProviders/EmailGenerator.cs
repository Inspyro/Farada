using System;
using System.ComponentModel.DataAnnotations;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  public class EmailGenerator:AttributeValueProvider<string, EmailAddressAttribute>
  {
    protected override string CreateValue (AttributeValueProviderContext<string, EmailAddressAttribute> context)
    {
      return "some@gmx.at";
    }
  }
}
