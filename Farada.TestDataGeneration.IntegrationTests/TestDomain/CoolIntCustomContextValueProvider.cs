using System;
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

    protected override int CreateValue (ExtendedValueProviderContext<int, ClassWithAttribute.CoolIntAttribute> context)
    {
      return _additionalValue + context.AdditionalData.Value;
    }
  }
}