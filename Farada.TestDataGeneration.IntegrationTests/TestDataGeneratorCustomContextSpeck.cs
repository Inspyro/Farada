using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using FluentAssertions;
using TestFx;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), "TODO")]
  class TestDataGeneratorCustomContextSpeck:TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorCustomContextSpeck ()
    {
       Specify (x =>
          TestDataGenerator.Create<int> (MaxRecursionDepth, null))
          .Case ("should fill all according to context", _ => _
              .Given (CustomContext (20))
              .It ("should fill int", x => x.Result.Should ().Be (20)));

      Specify (x =>
          TestDataGenerator.Create<ClassWithAttribute> (MaxRecursionDepth, null))
          .Case ("should fill all according to context", _ => _
              .Given (CustomAttributeContext (20))
              .It ("should fill int", x => x.Result.AttributedInt.Should ().Be (31)));
    }

     Context CustomContext (int contextValue)
    {
      return c => c.Given ("simple domain provider using custom context", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
          .UseDefaults(false)
            .For<int> ().AddProvider (new IntProviderWithCustomContext (contextValue));
      })
      .Given(TestDataGeneratorContext());
    }

    Context CustomAttributeContext (int contextValue)
    {
      return c => c.Given ("simple domain provider using custom context", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
          .UseDefaults(false)
            .For<int, ClassWithAttribute.CoolIntAttribute> ().AddProvider (new CoolIntCustomContextValueProvider (contextValue));
      })
          .Given (TestDataGeneratorContext ());
    }
  }
}
