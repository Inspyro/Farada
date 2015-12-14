using System;
using System.Collections.Generic;
using FakeItEasy;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.FakeItEasy;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.ValueProviders
{
  [Subject (typeof (ChooseSingleItemValueProvider<,>), "Create")]
  public class ChooseSingleItemValueProviderSpec : Spec
  {
    static List<int> InputList;
    [Faked] static IRandom Random;
    static TestDataDomainConfiguration TestDataDomainConfiguration;
    static ITestDataGenerator TestDataGenerator;

    public ChooseSingleItemValueProviderSpec ()
    {
      Specify (x => TestDataGenerator.Create<string> ())
          .Case ("when creating", _ => _
              .Given (BaseContext ())
              .Given ("Fake Random", c => SetupFakeRandom ())
              .It ("returns 2", x => x.Result.Should ().Be ("2")));
    }

    void SetupFakeRandom ()
    {
      A.CallTo (() => Random.Next (0, InputList.Count)).ReturnsNextFromSequence (1);
    }

    Context BaseContext ()
    {
      return
          c => c
              .Given ("{0,2,1}", x => InputList = new List<int> { 0, 2, 1 })
              .Given ("Empty domain with value provider", x =>
                  TestDataDomainConfiguration = (context => context
                      .UseDefaults (false)
                      .UseRandom (Random)
                      .For<string> ().AddProvider (new ChooseSingleItemValueProvider<int, string> (InputList, item => item.ToString ()))
                      .DisableAutoFill ()))
              .Given ("TestDataGenerator", x => TestDataGenerator = TestDataGeneratorFactory.Create (TestDataDomainConfiguration));
    }
  }
}