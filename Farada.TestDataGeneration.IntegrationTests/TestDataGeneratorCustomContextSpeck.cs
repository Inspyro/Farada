using System;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
  class TestDataGeneratorCustomContextSpeck:TestDataGeneratorBaseSpeck
  {
     Context CustomContext (int contextValue)
    {
      return c => c.Given ("simple domain provider using custom context", x =>
      {
        Domain = new DomainConfiguration
                 {
                    UseDefaults = false,
                     BuildValueProvider = builder => builder.AddProvider ((int i) => i, new IntProviderWithCustomContext(contextValue))
                 };
      })
      .Given(TestDataGeneratorContext());
    }

    [Group]
    void ValueProviderWithCustomContext ()
    {
      Specify (x =>
          TestDataGenerator.Create<int> (MaxRecursionDepth, null))
          .Elaborate ("should fill all according to context", _ => _
              .Given (CustomContext (20))
              .It ("should fill int", x => x.Result.Should ().Be (20)));
    }

    Context CustomAttributeContext (int contextValue)
    {
      return c => c.Given ("simple domain provider using custom context", x =>
      {
        Domain = new DomainConfiguration
                 {
                    UseDefaults = false,
                     BuildValueProvider = builder => builder.AddProvider ((int i, ClassWithAttribute.CoolIntAttribute cia) => i, new CoolIntCustomContextValueProvider(contextValue))
                 };
      })
      .Given(TestDataGeneratorContext());
    }

    [Group]
    void ValueProviderWithCustomAttributeContext ()
    {
      Specify (x =>
          TestDataGenerator.Create<ClassWithAttribute> (MaxRecursionDepth, null))
          .Elaborate ("should fill all according to context", _ => _
              .Given (CustomAttributeContext (20))
              .It ("should fill int", x => x.Result.AttributedInt.Should ().Be (31)));
    }
  }
}
