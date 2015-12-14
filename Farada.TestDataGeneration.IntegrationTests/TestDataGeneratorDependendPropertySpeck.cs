using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using Farada.TestDataGeneration.IntegrationTests.Utils;
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
          .Case ("should fill dependend properties", _ => _
              .Given (SimpleDependencyContext ())
              .It ("fills main color", x => x.Result.MainColor.Should ().Be (Color.Green))
              .It ("fills weight", x => x.Result.Weight.Should ().Be (10))
              .It ("fills engine.PowerInNewtons", x => x.Result.Engine.PowerInNewtons.Should ().Be (5))
              .It ("fills name with dependencies", x => x.Result.Name.Should ().Be ("VehicleX (Color:Green, Weight:10)")))
          .Case ("should throw on cycles", _ => _
              .Given (CyclicDependencyContext ())
              .ItThrows (typeof (ArgumentException), "Cyclic dependency found"))
          .Case ("Should throw on missing dependency", _ => _
              .Given (MissingDependencyContext ())
              .ItThrows (typeof (NotSupportedException), "Could not auto-fill AirVehicle> (member Name). Please provide a value provider")
              .ItThrowsInner (typeof (ArgumentException), "Could not find key:'KEY on AirVehicle: Type: Color, Member: MainColor' " +
                                                          "in dependend property collection. Have you registered the dependency?"));

      Specify (x => "dummy")
          .Case ("should throw on deep dependencies", _ => _
              .Given (DeepDependencyContext ())
              .It ("throws correct exception", x => CreationException.Should ().BeOfType<ArgumentException> ())
              .It ("throws correct exception message", x => CreationException.Message.Should ().Be (
                  "Chain 'KEY on AirVehicle: Type: Engine, Member: Engine > Type: Single, Member: PowerInNewtons' is invalid. " +
                  "You can only add dependencies with a chain of length 1. 'Deep Property dependencies' are not supported at the moment.")));

      //TODO: DependcyMapping null...
      Specify (x => TestDataGenerator.Create<ImmutableIce> ())
          .Case ("should fill dependend ctor args", _ => _
              .Given (SimpleCtorDependencyContext ())
              .It ("fills temperature", x => x.Result.Temperature.Should ().Be (4))
              .It ("fills origin", x => x.Result.Origin.Should ().Be ("Antarctica (4)")))
          .Case ("should throw on cycles in ctor args", _ => _
              .Given (CyclicCtorDependencyContext ())
              .ItThrows (typeof (ArgumentException), "Cyclic dependency found"));
    }

    Context CyclicDependencyContext ()
    {
      return c => c.Given("cyclic dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults(false)
            .For<object>().AddProvider(new DefaultInstanceValueProvider<object>())
            .For<Engine>()
              .AddProvider(context => new JetEngine())
              .DisableAutoFill()
             //cycle: Name->Weight->Name
            .For((AirVehicle a) => a.Name).AddProvider(context =>"dummy", a => a.Weight)
            .For((AirVehicle a) => a.Weight).AddProvider(context => 10, a=>a.Name)
            .For((AirVehicle a) => a.MainColor).AddProvider(context => Color.Green);
      })
         .Given(TestDataGeneratorContext());
    }

    Context DeepDependencyContext()
    {
      return c => c.Given("deep dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults(false)
            .For<object>().AddProvider(new DefaultInstanceValueProvider<object>())
            .For<Engine>()
              .AddProvider(context => new JetEngine())
              .DisableAutoFill()
            //deep dependency: 
            .For((AirVehicle a) => a.Name).AddProvider(context => "dummy", a => a.Engine.PowerInNewtons)
            .For((AirVehicle a) => a.Weight).AddProvider(context => 10)
            .For((AirVehicle a) => a.MainColor).AddProvider(context => Color.Green);
      })
         .Given(TestDataGeneratorContext(catchExceptions:true));
    }

    Context SimpleDependencyContext ()
    {
      return c => c.Given ("simple dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<Engine> ()
            .AddProvider (context => new JetEngine { PowerInNewtons = 5 })
            .DisableAutoFill ()
            .For ((AirVehicle a) => a.Name)
            .AddProvider (
                context =>
                    $"VehicleX (Color:{context.GetDependendValue (a => a.MainColor)}," +
                    $" Weight:{context.GetDependendValue (a => a.Weight)})",
                a => a.MainColor, a => a.Weight)
            .For ((AirVehicle a) => a.Weight).AddProvider (context => 10)
            .For ((AirVehicle a) => a.MainColor).AddProvider (context => Color.Green);
      })
          .Given (TestDataGeneratorContext ());
      /*
      For<AirVehicle>()
         /*.AddMetadataProvider(c=>
         { 
           var weight = c.TestDataGenerator.Create((AirVehicle av) => av.Weight;
           var x=c.Tdg.Create<string>();
           
           returnValues.Set(a=>a.Weight, "test");
           //return new {Weight = "test", Name="trea"};
         }).ForSubProperty(a=>a.Weight).ForSubProerty(a=>a.Name).For<string>().*/
      /*.AddMetadataProviderFunc(a=>a.Weight, a=>a.MainColor);
     .For((AirVehicle a) => a.Weight).AddProvider(c=>c.GetMetadata(a=>a.Weight*2)
     .For((AirVehicle a) => a.MainColor).AddProvider(c=>c.Metadata.MainColor);

   */
    }

    Context MissingDependencyContext()
    {
      return c => c.Given("missing dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults(false)
            .For<object>().AddProvider(new DefaultInstanceValueProvider<object>())
            .For<Engine>()
              .AddProvider(context => new JetEngine { PowerInNewtons = 5 })
              .DisableAutoFill()
            
              //missing dependency: MainColor
            .For((AirVehicle a) => a.Name)
            .AddProvider(context => $"VehicleX (Color:{context.GetDependendValue(a => a.MainColor)}")
            .For((AirVehicle a) => a.Weight).AddProvider(context => 10)
            .For((AirVehicle a) => a.MainColor).AddProvider(context => Color.Green);
      })
          .Given(TestDataGeneratorContext());
    }

    Context CyclicCtorDependencyContext()
    {
      //TODO: Dependency mapping is null in ice.Temperature?...
      return c => c.Given("cyclic ctor dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            //cycle: origin -> temperature -> origin
            .For ((ImmutableIce ice) => ice.Origin)
            .AddProvider (context => "don't care", ice => ice.Temperature)
            .For ((ImmutableIce ice) => ice.Temperature).AddProvider (context => 4 /*don't care*/, ice =>ice.Origin);
      })
          .Given(TestDataGeneratorContext());
    }

    Context SimpleCtorDependencyContext ()
    {
      return c => c.Given ("simple ctor dependency domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For ((ImmutableIce ice) => ice.Origin)
            .AddProvider (context => $"Antarctica ({context.GetDependendValue (ice => ice.Temperature)})", ice => ice.Temperature)
            .For ((ImmutableIce ice) => ice.Temperature).AddProvider (context => 4);
      })
          .Given (TestDataGeneratorContext ());


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
}