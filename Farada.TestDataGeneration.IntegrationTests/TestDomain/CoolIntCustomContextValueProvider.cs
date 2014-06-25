using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class CoolIntCustomContextValueProvider : AttributeBasedValueProvider<int, ClassWithAttribute.CoolIntAttribute,CoolIntCustomContextValueProvider.CoolIntCustomContext>
  {
    private readonly int _additionalValue;

    public CoolIntCustomContextValueProvider (int additionalValue)
    {
      _additionalValue = additionalValue;
    }

    protected override CoolIntCustomContext CreateContext (ValueProviderObjectContext objectContext)
    {
      return new CoolIntCustomContext (objectContext, _additionalValue);
    }

    protected override int CreateValue (CoolIntCustomContext context)
    {
      return context.AdditionalValue + context.Attribute.Value;
    }

    internal class CoolIntCustomContext : AttributeValueProviderContext<int, ClassWithAttribute.CoolIntAttribute>
    {
      public int AdditionalValue { get; private set; }

      protected internal CoolIntCustomContext (ValueProviderObjectContext objectContext, int additionalValue)
          : base (objectContext)
      {
        AdditionalValue = additionalValue;
      }
    }

  }
}