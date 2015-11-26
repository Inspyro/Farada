using System;
using System.Linq;
using Farada.Evolution.IntegrationTests.TestDomain;
using Farada.Evolution.RuleBasedDataGeneration;
using Farada.TestDataGeneration;
using FluentAssertions;
using TestFx.SpecK;
using Context = TestFx.SpecK.Context;

namespace Farada.Evolution.IntegrationTests
{
  [TestFx.SpecK.Subject (typeof (RuleBasedDataGenerator),"Generate")]
  public class RuleBasedDataGeneratorSpeck : Spec
  {
    TestDataDomainConfiguration TestDataDomain=null;
    EvolutionaryDomainConfiguration EvolutionaryDomain;
    RuleBasedDataGenerator DataGenerator;
    GeneratorResult InitialData;
    int Generations;

    public RuleBasedDataGeneratorSpeck ()
    {
        Specify (x => DataGenerator.Generate (1, InitialData))
          .Case ("String Church", _ => _
              .Given (StringDomainContext (true, 4))
              .It ("successfully gets more than 50% of the strings married",
                  x => x.Result.GetResult<string> ().Count (resultString => resultString.Contains ("Married to")).Should().Be (748)));

      Specify (x => DataGenerator.Generate (Generations, InitialData))
          .Case ("Planet Earth", _ => _
              .Given (PersonDomainContext ())
              .Given ("60 years", x => Generations = 60)
              .It ("successfully creates 1353 persons",
                  x => x.Result.GetResult<Person> ().Count.Should ().Be (1353)));
    }

    Context DataGeneratorContext ()
    {
      return
          c =>
              c.Given ("create rule based data generator",
                  x => DataGenerator = EvolutionaryDataGeneratorFactory.Create (TestDataDomain, EvolutionaryDomain));
    }

    Context StringInitialDataContext ()
    {
      return c => c.Given ("1000 sexy strings", x =>
      {
        var initialDataProvider = DataGenerator.InitialDataProvider;
        for (var i = 0; i < 1000; i++)
        {
          initialDataProvider.Add ("some sexy string " + i);
        }

        InitialData = initialDataProvider.Build ();
      });
    }

    Context StringDomainContext (bool useDefaults, int seed)
    {
      return c => c
          .Given ("string base domain", x => TestDataDomain = configurator => configurator.UseDefaults (useDefaults).UseRandom(new Random(seed)))
          .Given ("string evolutionary domain", x => EvolutionaryDomain = configurator => configurator.AddRule (new StringMarryRule ()))
          .Given (DataGeneratorContext ())
          .Given (StringInitialDataContext ());
    }

    Context PersonInitialDataContext ()
    {
      return c => c.Given ("adam and eve", x =>
      {
        var initialDataProvider = DataGenerator.InitialDataProvider;
        initialDataProvider.Add (new Person ("Adam", Gender.Male));
        initialDataProvider.Add (new Person ("Eve", Gender.Female));

        InitialData = initialDataProvider.Build ();
      });
    }

    Context PersonDomainContext (int seed = 0, bool useDefaults = true)
    {
      return c => c
          .Given ("person test domain", x => TestDataDomain = configurator => configurator
              //
              .UseDefaults (useDefaults).UseRandom (new Random (seed))
              //
              .For<Gender> ().AddProvider (context => (Gender) (context.Random.Next (0, 2))))
          .Given ("person evolution domain", x => EvolutionaryDomain = configurator => configurator
              //
              .AddGlobalRule (new WorldRule ())
              .AddRule (new ProcreationRule ()).AddRule (new AgingRule ()))
          .Given (DataGeneratorContext ())
          .Given (PersonInitialDataContext ());
    }
  }
}