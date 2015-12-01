using System;
using Farada.TestDataGeneration.CompoundValueProviders;
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
              .ItThrows (typeof (ArgumentException), "Cyclic dependency found"));

      Specify (x => "dummy")
          .Case ("should throw on deep dependencies", _ => _
              .Given (DeepDependencyContext ())
              .It ("throws correct exception", x => CreationException.Should ().BeOfType<ArgumentException> ())
              .It ("throws correct exception message", x => CreationException.Message.Should ().Be (
                  "Chain 'KEY on AirVehicle: Type: Engine, Member: Engine > Type: Single, Member: PowerInNewtons' is invalid. " +
                  "You can only add dependencies with a chain of length 1. 'Deep Property dependencies' are not supported at the moment.")));
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
    }
  }
}