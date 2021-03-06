﻿using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), nameof(String))]
  class TestDataGeneratorCustomContextSpeck:TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorCustomContextSpeck ()
    {
       Specify (x =>
          TestDataGenerator.Create<int> (MaxRecursionDepth, null))
          .Case ("should fill all according to context1", _ => _
              .Given (CustomContext (20))
              .It ("should fill int", x => x.Result.Should ().Be (20)));

      Specify (x =>
          TestDataGenerator.Create<ClassWithAttribute> (MaxRecursionDepth, null))
          .Case ("should fill all according to context2", _ => _
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
            .UseDefaults (false)
            .For<ClassWithAttribute> ().AddProvider (new DefaultInstanceValueProvider<ClassWithAttribute> ())
            .For<int> ().AddProvider(new CoolIntCustomContextValueProvider (contextValue));
      })
          .Given (TestDataGeneratorContext ());
    }
  }
}
