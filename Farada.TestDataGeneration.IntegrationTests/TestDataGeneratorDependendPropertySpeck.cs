using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), "Create")]
  public class TestDataGeneratorDependendPropertySpeck : TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorDependendPropertySpeck ()
    {
      Specify (x => TestDataGenerator.Create<AirVehicle> ())
          .Case ("should fill by metadata", _ => _
              .Given (SimpleMetadataContext (weight: 15, color: Color.Red))
              .It ("fills name with metadata", x => x.Result.Name.Should ().Be ("VehicleX (Color:Red, Weight:15)")))
          .Case ("should fill dependend properties", _ => _
              .Given (SimpleDependencyContext ())
              .It ("fills main color", x => x.Result.MainColor.Should ().Be (Color.Green))
              .It ("fills weight", x => x.Result.Weight.Should ().Be (10))
              .It ("fills engine.PowerInNewtons", x => x.Result.Engine.PowerInNewtons.Should ().Be (5))
              .It ("fills name with dependencies", x => x.Result.Name.Should ().Be ("VehicleX (Color:Green, Weight:10)")))
          .Case ("should fill pass through properties", _ => _
              .Given (PassThroughDependencyContext ())
              .It ("fills main color", x => x.Result.MainColor.Should ().Be (Color.Green))
              .It ("fills name with dependencies", x => x.Result.Name.Should ().Be ("VehicleX (Color:Green)")))
          .Case ("Should throw on missing dependency", _ => _
              .Given (MissingDependencyContext ())
              .ItThrows (typeof (ArgumentException),
                  "Could not find key:'KEY on AirVehicle: Type: Color, Member: MainColor' in metadata context. " +
                  "Have you registered the dependency before the metadata provider?"))
          .Case ("should throw on cycles", _ => _
              .Given (CyclicDependencyContext ())
              .ItThrows (typeof (ArgumentException), "Could not find key:'KEY on AirVehicle: Type: Int32, Member: Weight' in metadata context. " +
                                                     "Have you registered the dependency before the metadata provider?"))
          .Case ("should throw on deep dependencies", _ => _
              .Given (DeepDependencyContext ())
              .ItThrows (typeof (ArgumentException),
                  "Could not find key:'KEY on AirVehicle: Type: Engine, Member: Engine > Type: Single, Member: PowerInNewtons' in metadata context. "
                  +
                  "Have you registered the dependency before the metadata provider?"));

      Specify (x => TestDataGenerator.Create<ImmutableIce> ())
          .Case ("should fill ctor args according to metadata", _ => _
              .Given (SimpleCtorMetadataContext (temperature: 4))
              .It ("fills temperature", x => x.Result.Temperature.Should ().Be (1))
              .It ("fills origin", x => x.Result.Origin.Should ().Be ("Antarctica (4)")))
          .Case ("should fill dependend ctor args", _ => _
              .Given (SimpleCtorDependencyContext ())
              .It ("fills temperature", x => x.Result.Temperature.Should ().Be (4))
              .It ("fills origin", x => x.Result.Origin.Should ().Be ("Antarctica (4)")))
          .Case ("should throw on cycles in ctor args", _ => _
              .Given (CyclicCtorDependencyContext ())
              .ItThrows (typeof (ArgumentException),
                  //REVIEW: We get a different message here because the DefaultInstanceValueProvider does not construct the metadata on the wrong registration order...
                  "Could not find metadata context for key:'KEY on Farada.TestDataGeneration.IntegrationTests.TestDomain.ImmutableIce: Type: String, Member: Origin' . "
                  +
                  "Have you registered the dependency before the metadata provider?"));
    }

    Context SimpleMetadataContext (int weight, Color color)
    {
      return c => c.Given ("simple dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<Engine> ().AddProvider (context => new JetEngine { PowerInNewtons = 5 })
            .For<AirVehicle> ()
            .Select (a => a.Weight).AddProvider (context => 0)
            .Select (a => a.MainColor).AddProvider (context => Color.White)
            .For<AirVehicle> ().WithMetadata (ctx => new { Weight = weight, Color = color })
            .Select (a => a.Name).AddProvider (context => $"VehicleX (Color:{context.Metadata.Color}," + $" Weight:{context.Metadata.Weight})");
      })
          .Given (TestDataGeneratorContext ());
    }

    Context SimpleDependencyContext ()
    {
      return c => c.Given ("simple dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<Engine> ().AddProvider (context => new JetEngine { PowerInNewtons = 5 })
            .For<AirVehicle> ()
            .Select (a => a.Weight).AddProvider (context => 10)
            .Select (a => a.MainColor).AddProvider (context => Color.Green)
            .For<AirVehicle> ().WithMetadata (ctx => new { Weight = ctx.Get (a => a.Weight), Color = Color.Green })
            .Select (a => a.Name).AddProvider (context => $"VehicleX (Color:{context.Metadata.Color}," + $" Weight:{context.Metadata.Weight})");
      })
          .Given (TestDataGeneratorContext ());
    }

    Context PassThroughDependencyContext()
    {
      return c => c.Given("simple dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<Engine> ().AddProvider (context => new JetEngine { PowerInNewtons = 5 })
            .For<AirVehicle> ()
            .Select(a => a.Weight).AddProvider(context => 0)
            .Select (a => a.MainColor).AddProvider (context => Color.Green)
            .For<AirVehicle> ().WithMetadata (ctx=>ctx)
            .Select (a => a.Name).AddProvider (context => $"VehicleX (Color:{context.Metadata.Get (a => a.MainColor)})");
      })
          .Given(TestDataGeneratorContext());
    }

    Context CyclicDependencyContext ()
    {
      return c => c.Given ("cyclic dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<Engine> ()
            .AddProvider (context => new JetEngine ())
            //cycle: Name->Weight->Name
            .For<AirVehicle> ()
            .Select (a => a.MainColor).AddProvider (ctx => Color.White) //colors can't be contstructed
            .For<AirVehicle> ().WithMetadata (ctx => new { Weight = ctx.Get (a => a.Weight) })
            .Select (a => a.Name).AddProvider (context => context.Metadata.Weight.ToString ())
            .For<AirVehicle> ().WithMetadata (ctx => new { Name = ctx.Get (a => a.Name) })
            .Select (a => a.Weight).AddProvider (context => int.Parse (context.Metadata.Name));
      })
          .Given (TestDataGeneratorContext ());
    }

    Context MissingDependencyContext ()
    {
      return c => c.Given ("missing dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<Engine> ()
            .AddProvider (context => new JetEngine { PowerInNewtons = 5 })
            .For<AirVehicle> ()
            .Select (a => a.Weight).AddProvider (ctx => 0)
            .Select (a => a.Name).AddProvider (ctx => "")
            //missing dependency: MainColor
            .For<AirVehicle> ().WithMetadata (ctx => new { MainColor = ctx.Get (a => a.MainColor) })
            .Select (a => a.Name).AddProvider (context => context.Metadata.MainColor.ToString ())
            .For<AirVehicle> ()
            .Select (a => a.MainColor).AddProvider (ctx => Color.White); //this is too late.
      })
          .Given (TestDataGeneratorContext ());
    }

    Context DeepDependencyContext ()
    {
      return c => c.Given ("deep dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<Engine> ()
            .AddProvider (context => new JetEngine ())
            //deep dependency
            .For<AirVehicle> ()
            .Select (a => a.Weight).AddProvider (context => 10)
            .Select (a => a.MainColor).AddProvider (context => Color.Green)
            .Select (a => a.Engine.PowerInNewtons).AddProvider (context => 10f)
            .For<AirVehicle> ().WithMetadata (ctx => ctx.Get (a => a.Engine.PowerInNewtons))
            .Select (a => a.Name).AddProvider (context => context.Metadata.ToString ());
      })
          .Given (TestDataGeneratorContext (catchExceptions: true));
    }

    Context SimpleCtorMetadataContext (int temperature)
    {
      return c => c.Given ("simple ctor dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<ImmutableIce>()
            .Select (ice => ice.Temperature).AddProvider (context => 1)
            .For<ImmutableIce> ().WithMetadata (ctx => temperature)
            .Select (ice => ice.Origin).AddProvider (context => $"Antarctica ({context.Metadata})");
      })
          .Given (TestDataGeneratorContext ());
    }

    Context SimpleCtorDependencyContext ()
    {
      return c => c.Given ("simple ctor dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<ImmutableIce> ()
            .Select (ice => ice.Temperature).AddProvider (context => 4)
            .For<ImmutableIce> ().WithMetadata (ctx => ctx.Get (i => i.Temperature))
            .Select (ice => ice.Origin).AddProvider (context => $"Antarctica ({context.Metadata})");
      })
          .Given (TestDataGeneratorContext ());
    }

    Context CyclicCtorDependencyContext ()
    {
      return c => c.Given ("cyclic ctor dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            //cycle: origin -> temperature -> origin
            .For<ImmutableIce> ().WithMetadata(ctx=>ctx.Get(ice=>ice.Temperature))
            .Select (ice => ice.Origin).AddProvider (context => "don't care")
            .For<ImmutableIce>().WithMetadata(ctx=>ctx.Get(ice=>ice.Origin))
            .Select (ice => ice.Temperature).AddProvider (context => 4 /*don't care*/);
      })
          .Given (TestDataGeneratorContext ());
    }


    //return c => c.Given("simple ctor dependency domain", x =>
    //{
    //  TestDataDomainConfiguration = configurator => configurator.UseDefaults(false)
    //      .For<Order>().AddProvider (new DefaultInstanceValueProvider<object> ())
    //      .For((Order o) => ice.PaymentMethod, ((Order o) => ice.PaymentAcceptedCurrencies)
    //        .AddProvider(context => {
    //           var result = RandomPaymentMethodGenerator.Next();
    //           // ignored: result.PaymentProviderCompany
    //           return Tuple.Create(result.PaymentMethod, result.PaymentAcceptedCurrencies);
    //       });


    /* For<RevokeEvent>().AddProvider<User>(c=>c.User.ChangeEvent = changeEvent);
      *  For<RevokeEvent>(e=>e.AggregateId).AddProvider(c=>c.GetDependendValue(User.ChangeEvent).AggregateId, c.User);
      */
    /*
       return c => c.Given("simple ctor dependency domain", x =>
       {
         TestDataDomainConfiguration = configurator => configurator.UseDefaults(false)
             .For<Order>()
             .AddMetaProvider(context => RandomPaymentMethodGenerator.Next())
             .AddProvider (new DefaultInstanceValueProvider<object> ())
             .AddProvider (c => new ImmutableOrder(c.Metadata.PaymentMethod))
             .For((Order o) => ice.PaymentMethod).AddProvider(c => c.MetaData.PaymentMethod)
             .For((Order o) => ice.PaymentAcceptedCurrencies).AddProvider(c => c.MetaData.PaymentAcceptedCurrencies)
       })
      */


    /*
  return c => c.Given("simple ctor dependency domain", x =>
  {
    TestDataDomainConfiguration = configurator => configurator.UseDefaults(false)
        .For<Order>()
        .AddMetaProvider(context => new { A = "a" })
        .AddProvider (new DefaultInstanceValueProvider<object> ())
        .AddProvider (c => new ImmutableOrder(c.Metadata.A))
  })
 */


    /*
        ValueProviderContext.Metadata : TMetadata - provided per instance with AddMetadataProvider.
      */
  }
}