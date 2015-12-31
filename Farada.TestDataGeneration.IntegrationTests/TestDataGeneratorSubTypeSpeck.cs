using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Exceptions;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;
using FluentAssertions;
using TestFx.SpecK;

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
              .It ("should fill jet engine fuel per second", x => ((JetEngine) x.Result.Engine).FuelUsePerSecond.Should ().Be (20)));

      Specify (x =>
          TestDataGenerator.Create<LandVehicle> (MaxRecursionDepth, null))
          .Case ("should throw when creating LandVehicle", _ => _
              .Given (VehicleOnlyValueProviderContext ())
              .ItThrows (typeof (MissingValueProviderException), "No value provider registered for \"LandVehicle>\""));

      Specify (x =>
          TestDataGenerator.Create<AirVehicle> (MaxRecursionDepth, null))
          .Case ("should throw when creating AirVehicle", _ => _
              .Given (VehicleOnlyValueProviderContext ())
              .ItThrows (typeof (MissingValueProviderException), "No value provider registered for \"AirVehicle>\""));


      Specify (x =>
          TestDataGenerator.Create<CustomVehicle> (MaxRecursionDepth, null))
          .Case ("should fill unwrappable nullabe", _ => _
              .Given (ValueProviderNullableSubTypeContext (vectorId: "SomeVector"))
              .It ("should fill current direction",
                  x => x.Result.CurrentDirection.Should ().Be (new Vector { Id = "SomeVector" })));
    }

    Context ValueProviderSubTypeContext ()
     {
       return c => c.Given ("domain provider with sub type provider", x =>
       {
         TestDataDomainConfiguration = configurator => configurator
           .UseDefaults(false)
             .For<AbstractVehicle> ().AddProvider (new VehicleSubTypeProvider ());
       })
           .Given (TestDataGeneratorContext ());
     }

    Context ValueProviderNullableSubTypeContext (string vectorId)
    {
      return c => c.Given ("domain provider for nullables with sub type provider", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
            .UseDefaults (false)
            .For<AbstractVehicle> ().AddProvider (new DefaultInstanceValueProvider<AbstractVehicle> ())
            .For<int>().AddProvider(ctx=>0)
            .For<string>().AddProvider(ctx=>string.Empty)
            .For<Color>().AddProvider(ctx=>Color.White)
            .For<IVector> ().AddProvider (new SubTypeVectorProvider(vectorId));
      })
          .Given (TestDataGeneratorContext ());
    }

    Context VehicleOnlyValueProviderContext ()
    {
      return c => c.Given ("domain provider with sub type provider", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
          .UseDefaults(false)
            .For<AbstractVehicle> ().AddProvider (new VehicleOnlyValueProvider ());
      })
          .Given (TestDataGeneratorContext ());
    }
  }

  class SubTypeVectorProvider : SubTypeValueProvider<IVector>
  {
    readonly string _vectorId;

    public SubTypeVectorProvider (string vectorId)
    {
      _vectorId = vectorId;
    }

    protected override IVector CreateValue (ValueProviderContext<IVector> context)
    {
      return new Vector { Id = _vectorId };
    }

    public override ValueFillMode FillMode => ValueFillMode.FillAll;
  }
}
