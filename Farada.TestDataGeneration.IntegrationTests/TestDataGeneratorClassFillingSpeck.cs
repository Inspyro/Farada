using System;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Fluent;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using Farada.TestDataGeneration.IntegrationTests.Utils;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator),"Create")]
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
              .It ("should fill weight as specified", x => x.Result.Weight.Should ().Be (5))
              .It ("should fill main color as specified", x => x.Result.MainColor.Should ().Be (Color.Green))
              .It ("should fill engine with jetengine", x => x.Result.Engine.Should ().BeOfType (typeof (JetEngine)))
              .It ("should fill fuel use per second as specified",
                  x => ((JetEngine) x.Result.Engine).FuelUsePerSecond.Should ().Be (1.1f))
              .It ("should fill powerinnewtons as specified", x => x.Result.Engine.PowerInNewtons.Should ().Be (5000f)));

      Specify (x =>
          TestDataGenerator.Create<AirVehicle> (MaxRecursionDepth, null))
          .Case ("throws for abstract properties because of default provider chain", _ => _
              .Given (BaseDomainContext ())
              .ItThrows (typeof (NotSupportedException), "Could not auto-fill AirVehicle> (member Engine). Please provide a value provider")
              .ItThrowsInner (typeof (NotSupportedException),
                  "No valid ctor found on 'Engine': Classes with non-public constructors and abstract classes are not supported."));


      Specify (x =>
          TestDataGenerator.Create<LandVehicle> (MaxRecursionDepth, null))
          .Case ("should fill properties according to provider chain3", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              //test simple cases again because of more complex domain
              .It ("should fill weight as specified", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill main color as specified", x => x.Result.MainColor.Should ().Be (Color.Black))
              .It ("should fill tire diameter as specified", x => x.Result.Tire.Diameter.Should ().Be (1.2))
              .It ("should fill grip  as specified", x => x.Result.Tire.Grip.Should ().Be (1.2)));

      Specify (x =>
          TestDataGenerator.CreateMany<AirVehicle> (2,MaxRecursionDepth, null).First())
          .Case ("should fill properties according to provider chain4", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              //test simple cases again because of more complex domain
              .It ("should fill weight as specified", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill main color as specified", x => x.Result.MainColor.Should ().Be (Color.Black))

              //start testing concrete domain logic
              .It ("should fill engine with PropellorEngine", x => x.Result.Engine.Should ().BeOfType (typeof (PropellorEngine)))
              .It ("should fill fuel use per second as specified",
                  x => 
                    ((PropellorEngine) x.Result.Engine).AverageRotationSpeed.Should ().Be (2.1f))
              .It ("should fill PowerInNewtons as specified", x => x.Result.Engine.PowerInNewtons.Should ().Be (250f)));

      Specify (x =>
          TestDataGenerator.CreateMany<AirVehicle> (2, MaxRecursionDepth, null).Last())
          .Case ("should fill properties according to provider chain5", _ => _
              .Given (HierarchyPropertyProviderContext ())
              //
              //test simple cases again because of more complex domain
              .It ("should fill weight as specified", x => x.Result.Weight.Should ().Be (50))
              .It ("should fill main color as specified", x => x.Result.MainColor.Should ().Be (Color.Black))

              //start testing concrete domain logic
              .It ("should fill engine with jetengine", x => x.Result.Engine.Should ().BeOfType (typeof (JetEngine)))
              .It ("should fill fuel use per second as specified", x => ((JetEngine) x.Result.Engine).FuelUsePerSecond.Should ().Be (2.1f))
              .It ("should fill powerinnewtons as specified", x => x.Result.Engine.PowerInNewtons.Should ().Be (1200)));

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

      Specify (x =>
          TestDataGenerator.Create<ClassWithMixedMembers> (MaxRecursionDepth, null))
          .Case ("should fill mixed properties correctly", _ => _
              .Given (BaseDomainContext (seed: 0))
              .It ("should fill public property", x => x.Result.PublicProperty.Should ().Be ("Vigpmuhj"))
              .It ("should fill public field", x => x.Result.PublicField.Should ().Be ("Zaxseww"))
              .It ("Should not fill private property", x => x.Result.GetPrivateProperty ().Should ().Be ("default"))
              .It ("Should not fill private field", x => x.Result.GetPrivateField ().Should ().Be ("default")));
    }

    Context SimpleStringContext (int recursionDepth)
    {
      return c => c.Given ("simple string domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<string> ().AddProvider (context => "SomeString");
      })
      .Given(TestDataGeneratorContext(recursionDepth));
    }

    Context PropertyProviderContext ()
    {
      return c => c.Given ("simple property domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
            .UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<int> ().AddProvider (context => 5)
            .For<double> ().AddProvider (context => 3.6)
            .For<float> ().AddProvider (context => 1.1f)
            .For<Color> ().AddProvider (context => Color.Green)
            .For<string> ().AddProvider (context => "some string")
            //no default float provider!
            .For<LandVehicle> ()
            .Select (lv => lv.Weight).AddProvider (context => 100)
            .Select (lv => lv.MainColor).AddProvider (context => Color.Red)
            .For<AirVehicle> ().Select (av => av.Engine).AddProvider (context => new JetEngine ()).EnableAutoFill()
            .For<JetEngine> ().Select (je => je.PowerInNewtons).AddProvider (context => 5000);
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
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<double> ().AddProvider (context => 1.2)
            .For<float> ().AddProvider (context => 2.1f)
            .For<string> ().AddProvider (context => "some string")
            .For<Color> ().AddProvider (context => Color.Black)
            .For<AbstractVehicle> ().Select (v => v.Weight).AddProvider (context => 50)
            .For<AirVehicle> ().Select (av => av.Engine).AddProvider (context =>
            {
              //alternate between engine types
              i++;
              return i % 2 == 0 ? (Engine) new JetEngine () : new PropellorEngine ();
            }).EnableAutoFill()
            .For<Engine> ().Select (e => e.PowerInNewtons).AddProvider (context => 1200)
            .For<PropellorEngine> ().Select (pe => pe.PowerInNewtons).AddProvider (context => 250);
      })
          .Given (TestDataGeneratorContext ());
    }

    Context AttributeProviderContext ()
    {
      return c => c.Given ("simple attribute domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
            .UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<double> ().AddProvider (context => 2.1)
            .For<string> ().AddProvider (context => "something")
            .For<Color> ().AddProvider (context => Color.White)
            .For<double> ().AddProvider<double, InitialValueForChainAttribute> (context => context.AdditionalData[0].BaseValue + 0.1d)
            .For<int> ().AddProvider<int, InitialValueForChainAttribute> (context => context.AdditionalData[0].BaseValue + 2);
      })
          .Given (TestDataGeneratorContext ());
    }

    Context TypeHierarchyChainProviderContext ()
    {
      return c => c.Given ("simple hierarchical type chained domain", x =>
      {
        TestDataDomainConfiguration = configuration => configuration
            .UseDefaults (false)
            .For<AbstractVehicle>().AddProvider(new DefaultInstanceValueProvider<AbstractVehicle>())
            .For<Tire>().AddProvider(new DefaultInstanceValueProvider<Tire>())
            .For<double>().AddProvider(context=>0.0)
            .For<int>().AddProvider(context=>0)
            .For<Color>().AddProvider(context=>Color.White)
            .For<string> ()
            .AddProvider (context => "8")
            .AddProvider (context => "7" + context.GetPreviousValue ())
            //
            .For<string> ()
            .AddProvider<string, InitialStringValueForChainAttribute> (context => "6" + context.AdditionalData[0].BaseValue + context.GetPreviousValue ())
            .AddProvider<string, InitialStringValueForChainAttribute> (context => "5" + context.AdditionalData[0].BaseValue + context.GetPreviousValue ())
            //
            .For < AbstractVehicle>().Select(v => v.Name)
            .AddProvider (context => "4" + context.GetPreviousValue ())
            .AddProvider (context => "3" + context.GetPreviousValue ())
            //
            .For< LandVehicle>().Select(av => av.Name)
            .AddProvider (context => "2" + context.GetPreviousValue ())
            .AddProvider (context => "1" + context.GetPreviousValue ());

      })
          .Given (TestDataGeneratorContext ());
    }
  }
}