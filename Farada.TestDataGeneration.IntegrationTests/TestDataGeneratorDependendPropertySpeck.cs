using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Fluent;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), nameof (ITestDataGenerator.Create))]
  public class TestDataGeneratorDependendPropertySpeck : TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorDependendPropertySpeck ()
    {
      //Immutable classes
      Specify (x => TestDataGenerator.Create<AirVehicle> ())
          .Case ("should fill by metadata", _ => _
              .Given ("simple metadata domain", x => TestDataDomainConfiguration = cfg => cfg
                  .For<Engine> ().AddProvider (context => new JetEngine { PowerInNewtons = 5 })
                  .For<AirVehicle> ().WithMetadata (ctx => new { Weight = 15, Color = Color.Red })
                  .Select (a => a.Name).AddProvider (context => $"VehicleX (Color:{context.Metadata.Color}," + $" Weight:{context.Metadata.Weight})"))
              .Given(TestDataGeneratorContext())
              .It ("fills name with metadata", x => x.Result.Name.Should ().Be ("VehicleX (Color:Red, Weight:15)")))
          //
          .Case ("should fill by reused metadata", _ => _
              .Given ("reused dependency domain", x => TestDataDomainConfiguration = cfg => cfg
                  .UseRandom (new DefaultRandom (0))
                  .For<Engine> ().AddProvider (context => new JetEngine { PowerInNewtons = 5 })
                  .For<AirVehicle> ().WithMetadata (ctx => ctx.Random.Next ())
                  .Select (a => a.Weight).AddProvider (context => context.Metadata)
                  .Select (a => a.Name).AddProvider (context => $"Vehicle (Weight:{context.Metadata})")
              )
              .Given (TestDataGeneratorContext ())
              .It ("fills weight with same metadata", x => x.Result.Weight.Should ().Be (1755192844))
              .It ("fills name with same metadata", x => x.Result.Name.Should ().Be ("Vehicle (Weight:1755192844)")))
          //
          .Case ("should fill dependend properties", _ => _
              .Given ("simple dependency domain", x => TestDataDomainConfiguration = cfg => cfg
                  .For<Engine> ().AddProvider (context => new JetEngine { PowerInNewtons = 5 })
                  .For<AirVehicle> ()
                  .Select (a => a.Weight).AddProvider (context => 10)
                  .For<AirVehicle> ().WithMetadata (ctx => new { Weight = ctx.Get (a => a.Weight), Color = Color.Green })
                  .Select (a => a.Name)
                  .AddProvider (context => $"VehicleX (Color:{context.Metadata.Color}," + $" Weight:{context.Metadata.Weight})"))
              .Given (TestDataGeneratorContext ())
              .It ("fills weight", x => x.Result.Weight.Should ().Be (10))
              .It ("fills name with dependencies", x => x.Result.Name.Should ().Be ("VehicleX (Color:Green, Weight:10)")))
          //
          .Case ("should fill pass through properties", _ => _
              .Given ("pass through metadata domain", x => TestDataDomainConfiguration = cfg => cfg
                  .For<Engine> ().AddProvider (context => new JetEngine { PowerInNewtons = 5 })
                  .For<AirVehicle> ()
                  .Select (a => a.MainColor).AddProvider (context => Color.Green)
                  .For<AirVehicle> ().WithMetadata (ctx => ctx)
                  .Select (a => a.Name).AddProvider (context => $"VehicleX (Color:{context.Metadata.Get (a => a.MainColor)})"))
              .Given (TestDataGeneratorContext ())
              .It("fills main color", x => x.Result.MainColor.Should ().Be (Color.Green))
              .It ("fills name with dependencies", x => x.Result.Name.Should ().Be ("VehicleX (Color:Green)")))
          //
          .Case ("Should throw on missing dependency", _ => _
              .Given ("missing dependency domain", x => TestDataDomainConfiguration = cfg => cfg
                  .For<Engine> ()
                  .AddProvider (context => new JetEngine { PowerInNewtons = 5 })
                  //missing dependency: MainColor
                  .For<AirVehicle> ().WithMetadata (ctx => new { MainColor = ctx.Get (a => a.MainColor) })
                  .Select (a => a.Name).AddProvider (context => context.Metadata.MainColor.ToString ())
                  .For<AirVehicle> ()
                  .Select (a => a.MainColor).AddProvider (ctx => Color.White) /*this is too late*/)
              .Given (TestDataGeneratorContext ())
              .ItThrows(typeof (ArgumentException),
                  "Could not find key:'AirVehicle.MainColor' in metadata context. " +
                  "Have you registered the dependency before the metadata provider?"))
          //
          .Case ("should throw on cycles", _ => _
              .Given ("cyclic dependency domain", x => TestDataDomainConfiguration = cfg => cfg
                  .For<Engine> ().AddProvider (context => new JetEngine ())
                  //cycle: Weight->Weight
                  .For<AirVehicle> ()
                  .Select (a => a.MainColor).AddProvider (ctx => Color.White) //colors can't be constructed
                  .For<AirVehicle> ().WithMetadata (ctx => new { Weight = ctx.Get (a => a.Weight) })
                  .Select (a => a.Weight).AddProvider (context => context.Metadata.Weight))
              .Given (TestDataGeneratorContext ())
              .ItThrows(typeof (ArgumentException), "Could not find key:'AirVehicle.Weight' in metadata context. " +
                                                    "Have you registered the dependency before the metadata provider?"))
          //
          .Case ("should throw on nested dependencies", _ => _
              .Given ("nested dependency domain", x => TestDataDomainConfiguration = cfg => cfg
                  .For<Engine> ()
                  .AddProvider (context => new JetEngine ())
                  //deep dependency
                  .For<AirVehicle> ()
                  .Select (a => a.Engine.PowerInNewtons).AddProvider (context => 10f)
                  .For<AirVehicle> ().WithMetadata (ctx => ctx.Get (a => a.Engine.PowerInNewtons))
                  .Select (a => a.Name).AddProvider (context => "don't care"))
              .Given (TestDataGeneratorContext (catchExceptions: true))
              .ItThrows(typeof (ArgumentException),
                  "Could not find key:'AirVehicle.Engine.PowerInNewtons' in metadata context. " +
                  "Have you registered the dependency before the metadata provider?"));
      Specify (x => TestDataGenerator.Create<ImmutableIce> ())
          .Case ("should fill ctor args according to metadata", _ => _
              .Given ("ctor metadata domain", x => TestDataDomainConfiguration = cfg => cfg
                  .For<ImmutableIce> ().WithMetadata (ctx => 4)
                  .Select (ice => ice.Origin).AddProvider (context => $"Antarctica ({context.Metadata})"))
              .Given (TestDataGeneratorContext ())
              .It("fills origin according to metadata", x => x.Result.Origin.Should ().Be ("Antarctica (4)")))
          //
          .Case ("should fill ctor args according to reuused metadata", _ => _
              .Given ("reused ctor metadata domain", x => TestDataDomainConfiguration = cfg => cfg
                  .UseRandom (new DefaultRandom (0))
                  .For<ImmutableIce> ().WithMetadata (ctx => ctx.Random.Next ())
                  .Select (ice => ice.Temperature).AddProvider (context => context.Metadata)
                  .Select (ice => ice.Origin).AddProvider (context => $"Antarctica ({context.Metadata})"))
              .Given (TestDataGeneratorContext ())
              .It("fills temperature with same metadata", x => x.Result.Temperature.Should ().Be (1559595546))
              .It ("fills origin with same metadata", x => x.Result.Origin.Should ().Be ("Antarctica (1559595546)")))
          //
          .Case ("should fill dependend ctor args", _ => _
              .Given ("ctor dependency domain", x => TestDataDomainConfiguration = cfg => cfg
                  .For<ImmutableIce> ()
                  .Select (ice => ice.Temperature).AddProvider (context => 4)
                  .For<ImmutableIce> ().WithMetadata (ctx => ctx.Get (i => i.Temperature))
                  .Select (ice => ice.Origin).AddProvider (context => $"Antarctica ({context.Metadata})"))
              .Given (TestDataGeneratorContext ())
              .It("fills temperature", x => x.Result.Temperature.Should ().Be (4))
              .It ("fills origin with dependencies", x => x.Result.Origin.Should ().Be ("Antarctica (4)")))
          //
          .Case ("should throw on cycles in ctor args", _ => _
              .Given ("cylcic ctor dependency domain", x => TestDataDomainConfiguration = cfg => cfg
                  //cycle: temperature -> temperature
                  .For<ImmutableIce> ().WithMetadata (ctx => ctx.Get (ice => ice.Temperature))
                  .Select (ice => ice.Temperature).AddProvider (context => context.Metadata))
              .Given (TestDataGeneratorContext ())
              .ItThrows(typeof (ArgumentException),
                  "Could not find key:'Farada.TestDataGeneration.IntegrationTests.TestDomain.ImmutableIce.Temperature' " +
                  "in metadata context. Have you registered the dependency before the metadata provider?"));
    }
  }
}