using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using FluentAssertions;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
   [Subject (typeof (ITestDataGenerator), "Create_Immutable")]
  public class TestDataGeneratorImmutabilitySpeck:TestDataGeneratorBaseSpeck
  {
     public TestDataGeneratorImmutabilitySpeck ()
    {
      Specify (x => TestDataGenerator.Create<Ice> (MaxRecursionDepth, null))
          .Case ("Properties Are Initialized", _ => _
              .Given (ConfigurationContext (cfg =>
                  cfg.UseDefaults (false)
                      .For ((Ice ice) => ice.Origin).AddProvider (f => "FixedOrigin") //IDEA - ForCtorArg("origin")
                      .For ((Ice ice) => ice.Temperature).AddProvider (f => -100)))
              .It ("initialized first property correctly", x => x.Result.Origin.Should ().Be ("FixedOrigin"))
              .It ("initialized second property correctly", x => x.Result.Temperature.Should ().Be (-100)));
    }

     Context ConfigurationContext (TestDataDomainConfiguration config)
     {
       return c => c
           .Given ("domain with valid configuration", x => { 
             TestDataDomainConfiguration = config; })
           .Given (TestDataGeneratorContext ());
     }
  }

  public class Ice
  {
    private readonly string _origin;
    private readonly int _temperature;

    public string Origin { get { return _origin;} }
    public int Temperature { get { return _temperature; } }

    public Ice (string origin, int temperature)
    {
      _origin = origin;
      _temperature = temperature;
    }
  }
}