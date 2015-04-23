using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using FluentAssertions;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), "Create_SubTypes")]
  class TestDataGeneratorSubTypeSpeck:TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorSubTypeSpeck ()
    {
      Specify (x =>
          TestDataGenerator.Create<LandVehicle> (MaxRecursionDepth, null))
          .Case ("should fill all according to context1", _ => _
              .Given (ValueProviderSubTypeContext ())
              .It ("should fill tire diameter", x => x.Result.Tire.Diameter.Should ().Be (10)));

       Specify (x =>
          TestDataGenerator.Create<AirVehicle> (MaxRecursionDepth, null))
          .Case ("should fill all according to context2", _ => _
              .Given (ValueProviderSubTypeContext ())
              .It ("should fill jet engine fuel per second", x => ((JetEngine)x.Result.Engine).FuelUsePerSecond.Should ().Be (20)));

      Specify (x =>
          TestDataGenerator.Create<LandVehicle> (MaxRecursionDepth, null))
          .Case ("should not fill landvehicle (no exception)", _ => _
              .Given (VehicleOnlyValueProviderContext ())
              .It ("should not create land vehicle as only abstract vehicle would be filled", x => x.Result.Tire.Should().BeNull()));

       Specify (x =>
          TestDataGenerator.Create<AirVehicle> (MaxRecursionDepth, null))
          .Case ("should not fill (no exception)", _ => _
              .Given (VehicleOnlyValueProviderContext ())
              .It ("should not create air vehicle as only abstract vehicle would be filled", x =>x.Result.Engine.Should().BeNull()));
    }
    
    Context ValueProviderSubTypeContext ()
     {
       return c => c.Given ("domain provider with sub type provider", x =>
       {
         TestDataDomainConfiguration = configurator => configurator
           .UseDefaults(false)
             .For<AbstractVehicle> ().AddProvider (new VehicleSubTypeProvider ());
       })
           .Given (TestDataGeneratorContext (RecursionDepth.DoNotFillSecondLevelProperties));
     }

    Context VehicleOnlyValueProviderContext ()
    {
      return c => c.Given ("domain provider with sub type provider", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
          .UseDefaults(false)
            .For<AbstractVehicle> ().AddProvider (new VehicleOnlyValueProvider ());
      })
          .Given (TestDataGeneratorContext (RecursionDepth.DoNotFillSecondLevelProperties));
    }
  }
}
