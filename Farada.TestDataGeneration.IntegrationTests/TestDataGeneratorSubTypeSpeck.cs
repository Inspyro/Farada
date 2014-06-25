using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using Farada.TestDataGeneration.Extensions;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
  class TestDataGeneratorSubTypeSpeck:TestDataGeneratorBaseSpeck
  {
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

    [Group]
    void ValueProviderForSubTypes()
    {
      Specify (x =>
          TestDataGenerator.Create<AbstractVehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill all according to context", _ => _
              .Given (ValueProviderSubTypeContext ())
              .It ("should fill tire diameter", x => x.Result.Tire.Diameter.Should ().Be (10)));

       Specify (x =>
          TestDataGenerator.Create<AbstractVehicle.AirVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should fill all according to context", _ => _
              .Given (ValueProviderSubTypeContext ())
              .It ("should fill jet engine fuel per second", x => ((AbstractVehicle.JetEngine)x.Result.Engine).FuelUsePerSecond.Should ().Be (20)));
    }

    Context VehicleOnlyValueProviderContext ()
    {
      return c => c.Given ("domain provider with sub type provider", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
          .UseDefaults(false)
            .For<AbstractVehicle> ().AddProvider (new VehicleOnlyValueProvider ());
      })
          .Given (TestDataGeneratorContext ((int) RecursionDepth.DoNotFillSecondLevelProperties));
    }

    [Group]
    void ValueProviderNotForSubTypes()
    {
      Specify (x =>
          TestDataGenerator.Create<AbstractVehicle.LandVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should not fill landvehicle (no exception)", _ => _
              .Given (VehicleOnlyValueProviderContext ())
              .It ("should not create land vehicle as only abstract vehicle would be filled", x => x.Result.Tire.Should().BeNull()));

       Specify (x =>
          TestDataGenerator.Create<AbstractVehicle.AirVehicle> (MaxRecursionDepth, null))
          .Elaborate ("should not fill (no exception)", _ => _
              .Given (VehicleOnlyValueProviderContext ())
              .It ("should not create air vehicle as only abstract vehicle would be filled", x =>x.Result.Engine.Should().BeNull()));
    }
  }
}
