using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Exceptions;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), "Create_Immutable")]
  public class TestDataGeneratorImmutabilitySpeck : TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorImmutabilitySpeck ()
    {
      Specify (x => TestDataGenerator.Create<ImmutableIce> (MaxRecursionDepth, null))
          .Case ("Properties Are Initialized", _ => _
              .Given (ConfigurationContext (cfg =>
                  cfg.UseDefaults (false)
                      .For<ImmutableIce> ().AddProvider (new DefaultInstanceValueProvider<ImmutableIce> ())
                      .For ((ImmutableIce ice) => ice.Origin).AddProvider (f => "FixedOrigin") //IDEA - ForCtorArg("origin")
                      .For ((ImmutableIce ice) => ice.Temperature).AddProvider (f => -100)))
              .It ("initialized first property correctly", x => x.Result.Origin.Should ().Be ("FixedOrigin"))
              .It ("initialized second property correctly", x => x.Result.Temperature.Should ().Be (-100)))
          .Case ("No value providers defined", _ => _
              .Given (ConfigurationContext (cfg =>
                  cfg.UseDefaults (false)
                      .For<ImmutableIce> ().AddProvider (new DefaultInstanceValueProvider<ImmutableIce> ())))
              .ItThrows (typeof (MissingValueProviderException),
                  "No value provider registered for \"KEY on Farada.TestDataGeneration.IntegrationTests.TestDomain.ImmutableIce: Type: String, Member: Origin\""));
    }

    Context ConfigurationContext (TestDataDomainConfiguration config)
    {
      return c => c
          .Given ("domain with valid configuration", x => { TestDataDomainConfiguration = config; })
          .Given (TestDataGeneratorContext ());
    }
  }
}