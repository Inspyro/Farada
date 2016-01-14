using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.ValueProviders
{
  [Subject (typeof (EmailGenerator), nameof(String))]
  public class EmailGeneratorSpec : Spec
  {
    static TestDataDomainConfiguration TestDataDomainConfiguration;
    static ITestDataGenerator TestDataGenerator;

    public EmailGeneratorSpec ()
    {
      Specify (x => TestDataGenerator.Create<ClassWithEmail> ())
          .Case ("when creating", _ => _
              .Given (BaseContext (seed: 5))
              .It ("returns 2", x => x.Result.Email.Should ().Be ("heri.yid@kacif.du")));
    }

    Context BaseContext (int seed)
    {
      return
          c => c
              .Given ("Empty domain with value provider", x =>
                  TestDataDomainConfiguration = (context => context
                      .UseDefaults (false)
                      .UseRandom (new DefaultRandom (seed))
                      .For<ClassWithEmail>().AddProvider(new DefaultInstanceValueProvider<ClassWithEmail>())
                      .For<ClassWithEmail> ().Select (cwe => cwe.Email).AddProvider (new EmailGenerator ())))
              .Given ("TestDataGenerator", x => TestDataGenerator = TestDataGeneratorFactory.Create (TestDataDomainConfiguration));
    }
  }

  public class ClassWithEmail
  {
    [EmailAddress]
    public string Email { get; set; }
  }
}