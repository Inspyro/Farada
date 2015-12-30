using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class CoolIntCustomContextValueProvider : AttributeBasedValueProvider<int, ClassWithAttribute.CoolIntAttribute>
  {
    readonly int _additionalValue;

    public CoolIntCustomContextValueProvider (int additionalValue)
    {
      _additionalValue = additionalValue;
    }

    protected override int CreateAttributeBasedValue (AttributeValueProviderContext<int, ClassWithAttribute.CoolIntAttribute> context)
    {
      return _additionalValue + context.Attribute.Value;
    }
  }
}