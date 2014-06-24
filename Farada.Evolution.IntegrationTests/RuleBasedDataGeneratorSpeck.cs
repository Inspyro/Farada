using System;
using System.Collections.Generic;
using System.Linq;
using Farada.Core.Extensions;
using Farada.Evolution.RuleBasedDataGenerator;
using Farada.Evolution.Utilities;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;

namespace Farada.Evolution.IntegrationTests
{
  [Subject (typeof (RuleBasedDataGenerator.RuleBasedDataGenerator))]
  public class RuleBasedDataGeneratorSpeck : Specs
  {
    DomainConfiguration Domain;
    bool UseDefaults;
    RuleBasedDataGenerator.RuleBasedDataGenerator DataGenerator;
    GeneratorResult InitialData;
    int Generations;

    Context DefaultsContext (bool useDefaults)
    {
      return c => c.Given ("use defaults " + useDefaults, x => UseDefaults = useDefaults);
    }


    Context DataGeneratorContext ()
    {
      return
          c =>
              c.Given ("create rule based data generator",
                  x => DataGenerator = Farada.CreateRuleBasedDataGenerator (Domain, UseDefaults));
    }

    Context BasePropertyContext (bool useDefaults = true)
    {
      return c => c.Given (DefaultsContext (useDefaults))
          .Given (DataGeneratorContext ());
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

    Context StringDomainContext (bool useDefaults = true)
    {
      return c => c
          .Given ("string domain", x => Domain = new DomainConfiguration
                                                 {
                                                     Rules = new RuleSet (new StringMarryRule ())
                                                 })
          .Given (BasePropertyContext (useDefaults))
          .Given (StringInitialDataContext ());
    }

    [Group]
    void SimpleStringDomain ()
    {
      Specify (x => DataGenerator.Generate (1, InitialData))
          .Elaborate ("String Church", _ => _
              .Given (StringDomainContext ())
              .It ("successfully gets 50% of the strings married",
                  x => x.Result.GetResult<string> ().Count (resultString => resultString.Contains ("Married to")).Should().Be (500)));
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

    Context PersonDomainContext (int seed=0, bool useDefaults = true)
    {
      return c => c
          .Given ("person domain", x =>
          {
            var lifeRuleSet = new RuleSet (new ProcreationRule (), new AgingRule ());
            lifeRuleSet.AddGlobalRule (new WorldRule ());
            Domain = new DomainConfiguration
                     {
                         Random = new Random (seed),
                         Rules = lifeRuleSet,
                         BuildValueProvider = builder => builder.AddProvider ((Gender g) => g, context => (Gender) (context.Random.Next (0, 2)))
                     };
          })
          .Given (BasePropertyContext (useDefaults))
          .Given (PersonInitialDataContext ());
    }

    [Group]
    void PersonDomain ()
    {
      Specify (x => DataGenerator.Generate (Generations, InitialData))
          .Elaborate ("Planet Earth", _ => _
              .Given (PersonDomainContext ())
              .Given ("50 years", x => Generations = 50)
              .It ("successfully creates 3991 persons",
                  x => x.Result.GetResult<Person> ().Count.Should ().Be (3991)));
    }

    #region Helper Code

    class StringMarryRule : Rule
    {
      public override float GetExecutionProbability ()
      {
        return 0.5f;
      }

      protected override IEnumerable<IRuleParameter> GetRuleInputs ()
      {
        Func<RuleValue<string>, bool> predicate = p => p.Value.Length > 3 && (p.UserData.IsMarried == null || !p.UserData.IsMarried);
        yield return new RuleParameter<string> (predicate);
        yield return new RuleParameter<string> (predicate); //TODO: how to define excludes on rule filter basis?
      }

      protected override IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData) //e.g. one instance - stores all generation data..
      {
        var sexyString1 = inputData.GetValue<string> (0);
        var sexyString2 = inputData.GetValue<string> (1);

        sexyString1.Value += "[Married to:" + sexyString2.Value + "]";
        sexyString2.Value += "[Married to:" + sexyString1.Value + "]";

        sexyString1.UserData.IsMarried = true;
        sexyString2.UserData.IsMarried = true;

        yield break;
      }
    }


    internal class WorldRule : GlobalRule
    {
      protected override void Execute ()
      {
        World.Write (x => x.Fertility = 100);
      }
    }

    //TODO: Rule for every generation - not per type
    //TODO: World class that contains global generation data - like World.IsFertile..
    internal class AgingRule : UnrestrictedRule<Person>
    {
      protected override IEnumerable<Person> Execute (RuleValue<Person> person)
      {
        person.UserData.IsPregnant = false;
        person.Value.Age++;

        yield break;
      }
    }

    internal class ProcreationRule : Rule
    {
      public override float GetExecutionProbability ()
      {
        //TODO: Rule Appliance Probability based on World.Fertility...
        var fertility = World.Read<int?> (x => x.Fertility);
       
        return LerpUtility.LerpFromLowToHigh (100000, World.Count<Person> (), 1f, 0.1f);
      }

      protected override IEnumerable<IRuleParameter> GetRuleInputs ()
      {
        //TODO: can we have relations between persons? a likes b,c,..  d hates f,g..?
        yield return new RuleParameter<Person> ( p => p.Value.Age >= 14 && p.Value.Gender == Gender.Male);
        yield return new RuleParameter<Person> (p => p.Value.Age >= 14 && p.Value.Gender == Gender.Female && (p.UserData.IsPregnant == null || !p.UserData.IsPregnant));

        //TODO: do we need optional rule paramters?
      }

      protected override IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData)
      {
        var male = inputData.GetValue<Person> (0);
        var female = inputData.GetValue<Person> (1);

        var childCount = 1; // TODO: Get Random object from somewhere ValueProvider.Random.Next(1, 1);
        for (var i = 0; i < childCount; i++)
        {
          var child = ValueProvider.Create<Person> ();
          child.Father = male.Value;
          child.Mother = female.Value;

          yield return new RuleValue<Person> (child);
        }

        female.UserData.IsPregnant = true;
      }
    }

    internal class Person
    {
      public string Name { get; set; }
      public int Age { get; set; }
      public Gender Gender { get; set; }

      public Person Father { get; set; }
      public Person Mother { get; set; }

      public Person ()
      {
      }

      public Person (string name, Gender gender, int age = 0)
      {
        Name = name;
        Gender = gender;
        Age = age;
      }
    }

    internal enum Gender
    {
      Male,
      Female
    }

    #endregion
  }
}