using System;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using FluentAssertions;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator),"TODO")]
  public class TestDataGeneratorClassFillingSpeck : TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorClassFillingSpeck ()
    {
      Specify (x =>
          TestDataGenerator.Create<Universe> (MaxRecursionDepth, null))
          .Case ("should fill normal property deep in hierarchy", _ => _
              .Given (SimpleStringContext (3))
              .It ("fills properties in 1st level deep hierarchy", x => x.Result.Galaxy1.StarSystem1.Planet1.President.Name.Should ().Be ("SomeString"))
              .It ("fill properties in 2nd level deep hierarchy",
                  x =>
                      x.Result.Galaxy1.StarSystem1.Planet1.President.Atom1.Particle1.QuantumUniverse.Galaxy1.StarSystem1.Planet1.President.Name.Should ()
                          .Be ("SomeString"))
              .It ("aborts hierarchy filling at 3rd level top element (QuantumUniverse)",
                  x =>
                      x.Result.Galaxy1.StarSystem1.Planet1.President.Atom1.Particle1.QuantumUniverse.Galaxy1.StarSystem1.Planet1.President.Atom1.Particle1
                          .QuantumUniverse.Galaxy1.Should ()
                          .BeNull ()));


      Specify (x =>
          TestDataGenerator.Create<LandVehicle> (MaxRecursionDepth, null))
          .Case ("should fill properties according to provider chain1", _ => _
              .Given (PropertyProviderContext ())
              .It ("should fill weight", x => x.Result.Weight.Should ().Be (100))
              .It ("should fill main color", x => x.Result.MainColor.Should ().Be (Color.Red))
              .It ("should fill tire diameter", x => x.Result.Tire.Diameter.Should ().Be (3.6))
              .It ("should fill grip", x => x.Result.Tire.Grip.Should ().Be (3.6)));

      Specify (x =>
          TestDataGenerator.Create<AirVehicle> (MaxRecursionDepth, null))
          .Case ("should fill properties according to provider chain2", _ => _
              .Given (PropertyProviderContext ())
              .It ("should fill weight with default int", x => x.Result.Weight.Should ().Be (5))
              .It ("should fill main color with first enum value", x => x.Result.MainColor.Should ().Be (Color.White))
              .It ("should fill engine with jetengine", x => x.Result.Engine.Should ().BeOfType (typeof (JetEngine)))
              .It ("should fill fuel use per second with default float",
                  x => ((JetEngine) x.Result.Engine).FuelUsePerSecond.Should ().Be (0f))
              .It ("should fill powerinnewtons as specified", x => x.Result.Engine.PowerInNewtons.Should ().Be (5000f)));

        Specify (x =>
          TestDataGenerator.Create<AirVehicle> (MaxRecursionDepth, null))
          .Case ("should not fill abstact properties because of default provider chain", _ => _
              .Given (BaseDomainContext ())
              .It ("should not fill abstract engine", x => x.Result.Engine.Should ().BeNull()));

      Specify (x =>
          TestDataGenerator.Create<LandVehicle> (MaxRecursionDepth, null))
          .Case ("should fill properties according to provider chain3", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              //test simple cases again because of more complex domain
              .It ("should fill weight", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill default color", x => x.Result.MainColor.Should ().Be (Color.White))
              .It ("should fill tire diameter with default value", x => x.Result.Tire.Diameter.Should ().Be (0))
              .It ("should fill grip with default value", x => x.Result.Tire.Grip.Should ().Be (0)));

      Specify (x =>
          TestDataGenerator.CreateMany<AirVehicle> (2,MaxRecursionDepth, null).First())
          .Case ("should fill properties according to provider chain4", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              //test simple cases again because of more complex domain
              .It ("should fill weight with specified int", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill main color with first enum value", x => x.Result.MainColor.Should ().Be (Color.White))

              //start testing concrete domain logic
              .It ("should fill engine with propellorengine", x => x.Result.Engine.Should ().BeOfType (typeof (PropellorEngine)))
              .It ("should fill fuel use per second with float",
                  x => 
                    ((PropellorEngine) x.Result.Engine).AverageRotationSpeed.Should ().Be (2.1f))
              .It ("should fill powerinnewtons as specified", x => x.Result.Engine.PowerInNewtons.Should ().Be (250f)));

      Specify (x =>
          TestDataGenerator.CreateMany<AirVehicle> (2, MaxRecursionDepth, null).Last())
          .Case ("should fill properties according to provider chain5", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              //test simple cases again because of more complex domain
              .It ("should fill weight with specified int", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill main color with first enum value", x => x.Result.MainColor.Should ().Be (Color.White))

              //start testing concrete domain logic
              .It ("should fill engine with jetengine", x => x.Result.Engine.Should ().BeOfType (typeof (JetEngine)))
              .It ("should fill fuel use per second with float", x => ((JetEngine) x.Result.Engine).FuelUsePerSecond.Should ().Be (2.1f))
              .It ("should fill powerinnewtons as with float", x => x.Result.Engine.PowerInNewtons.Should ().Be (1200)));

      Specify (x =>
          TestDataGenerator.Create<LandVehicle> (MaxRecursionDepth, null))
          .Case ("should fill properties according to provider chain6", _ => _
              .Given(AttributeProviderContext())
              .It ("should fill tire usage", x => x.Result.Tire.TireUsage.Should ().Be (100.1d))
              .It ("should fill weight", x => x.Result.Weight.Should ().Be (52)));

      Specify (x =>
          TestDataGenerator.Create<LandVehicle> (MaxRecursionDepth, null))
          .Case ("should fill properties according to provider chain7", _ => _
              .Given (TypeHierarchyChainProviderContext ())
              .It ("should fill name", x => x.Result.Name.Should ().Be ("12345!6!78")));
    }

    Context SimpleStringContext (int recursionDepth)
    {
      return c => c.Given ("simple string domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults(false)
          .For<string> ().AddProvider (context => "SomeString");
      })
      .Given(TestDataGeneratorContext(recursionDepth));
    }

    Context PropertyProviderContext ()
    {
      return c => c.Given ("simple property domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
          .UseDefaults(false)
            .For<int> ().AddProvider (context => 5)
            .For<double> ().AddProvider (context => 3.6)
            //no default float provider!
            .For ((LandVehicle lv) => lv.Weight).AddProvider (context => 100)
            .For ((LandVehicle lv) => lv.MainColor).AddProvider (context => Color.Red)
            .For ((AirVehicle av) => av.Engine).AddProvider (context => new JetEngine ())
            .For ((JetEngine je) => je.PowerInNewtons).AddProvider (context => 5000);
      })
          .Given (TestDataGeneratorContext ());
    }

    Context HierarchyPropertyProviderContext ()
    {
      return c => c.Given ("hierachical property domain", x =>
      {
        var i = 0;

        TestDataDomainConfiguration = configurator => configurator
            .UseDefaults (false)
            .For<float> ().AddProvider (context => 2.1f)
            .For ((AbstractVehicle v) => v.Weight).AddProvider (context => 50)
            .For ((AirVehicle av) => av.Engine).AddProvider (context =>
            {
              //alternate between engine types
              i++;
              return i % 2 == 0 ? (Engine) new JetEngine () : new PropellorEngine ();
            })
            .For ((Engine e) => e.PowerInNewtons).AddProvider (context => 1200)
            .For ((PropellorEngine pe) => pe.PowerInNewtons).AddProvider (context => 250);
      })
          .Given (TestDataGeneratorContext ());
    }

    Context AttributeProviderContext ()
    {
      return c => c.Given ("simple attribute domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
          .UseDefaults(false)
            .For<double, InitialValueForChainAttribute> ().AddProvider (context => context.Attribute.BaseValue + 0.1d)
            .For<int, InitialValueForChainAttribute> ().AddProvider (context => context.Attribute.BaseValue + 2);
      })
      .Given(TestDataGeneratorContext());
    }

    Context TypeHierarchyChainProviderContext ()
    {
      return c => c.Given ("simple hierachical type chained domain", x =>
      {
        TestDataDomainConfiguration = configuration => configuration
            .UseDefaults (false)
            .For<string> ()
            .AddProvider (context => "8")
            .AddProvider (context => "7" + context.GetPreviousValue ())
            //
            .For<string, InitialStringValueForChainAttribute> ()
            .AddProvider (context => "6" + context.Attribute.BaseValue + context.GetPreviousValue ())
            .AddProvider (context => "5" + context.Attribute.BaseValue + context.GetPreviousValue ())
            //
            .For ((AbstractVehicle v) => v.Name)
            .AddProvider (context => "4" + context.GetPreviousValue ())
            .AddProvider (context => "3" + context.GetPreviousValue ())
            //
            .For ((LandVehicle av) => av.Name)
            .AddProvider (context => "2" + context.GetPreviousValue ())
            .AddProvider (context => "1" + context.GetPreviousValue ());

      })
          .Given (TestDataGeneratorContext ());
    }
  }
}