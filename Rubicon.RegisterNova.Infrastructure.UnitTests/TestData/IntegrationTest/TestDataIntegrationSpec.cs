using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Rubicon.RegisterNova.Infrastructure.JetBrainsAnnotations;
using Rubicon.RegisterNova.Infrastructure.TestData;
using Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.String;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.UnitTests.TestData.IntegrationTest
{
  [Subject(typeof(TestDataGenerator))]
  internal class TestDataIntegrationSpec
  {
    class when_using_TypeValueProvider
    {
      Because of = () =>
      {
        var domain = new BaseDomainConfiguration
                     {
                         BuildValueProvider = builder =>
                         {
                           //TODO: decorator for base value provider.. - get value from chain, so do next provider first...?
                           //TODO: per type: 
                           builder.SetProvider(new StringGenerator());

                           builder.SetProvider((Dog d) => d.FirstName, new DogNameGenerator("first name"));
                           builder.SetProvider((Dog d) => d.LastName, new DogNameGenerator("last name"));
                           builder.SetProvider((Dog d)=> d.BestDogFriend.FirstName, new DogNameGenerator("dog friend first name"));

                           builder.SetProvider(new CatGenerator());

                           builder.SetProvider((Dog d) => d.BestDogFriend, new DogFriendInjector());
                           builder.SetProvider((Cat c) => c.Name, new FuncProvider<string>((random) => "cat name..."));
                         }
                     };

        ValueProvider = TestDataGeneratorFactory.CreateValueProvider(domain);
      };

      It sets_correctString =
          () => ValueProvider.Create<string>().ShouldEqual("Some random string...");

      It sets_correctDogFirstName = () => ValueProvider.Create<Dog>().FirstName.ShouldEqual("Dog Name String Gen - first name");

      It sets_correctDogLastName = () => ValueProvider.Create<Dog>().LastName.ShouldEqual("Dog Name String Gen - last name");

      It returns_correctDogType = () => ValueProvider.Create<Dog>().GetType().ShouldEqual(typeof (Dog));

      It sets_correctBestFriendDogFirstName =
          () => ValueProvider.Create<Dog>().BestDogFriend.FirstName.ShouldEqual("Dog Name String Gen - dog friend first name");

      It sets_correctBestFriendDogLastName = () => ValueProvider.Create<Dog>().BestDogFriend.LastName.ShouldEqual("Dog Name String Gen - last name");

      It returns_correctBestFriendDogType = () => ValueProvider.Create<Dog>().BestDogFriend.GetType().ShouldEqual(typeof (DogFriend));

      It sets_correctDogAge = () => ValueProvider.Create<Dog>().Age.ShouldEqual(0);

      It sets_correctBestCatFriendsName = () => ValueProvider.Create<Dog>().BestCatFriend.Name.ShouldEqual("cat name...");

      It sets_correctCatWithoutProperties = () => ValueProvider.Create<Cat>(0).Name.ShouldEqual("Nice cat");

      It sets_correctInt = () => ValueProvider.Create<int>().ShouldEqual(0);

      static CompoundValueProvider ValueProvider;
    }

    class when_using_TypeValueProvider_Performance
    {
      Because of = () =>
      {
        var basicDomain = new DomainConfiguration();
        var valueProvider = TestDataGeneratorFactory.CreateValueProvider(basicDomain);

        const int count = 1000000; //1 million

        var start = DateTime.Now;

        for (var i = 0; i < count; i++)
        {
          valueProvider.Create<Universe>();
        }

        Console.WriteLine("Took {0} s to generate {1} universes", (DateTime.Now - start).TotalSeconds, count);
      };

      It doesNothing = () => true.ShouldBeTrue();

    }

    class when_using_TypeValueProvider_Word
    {
      Because of = () =>
      {
        var domain = new BaseDomainConfiguration
                     {
                         BuildValueProvider = builder =>
                         {
                           builder.SetProvider(new RandomWordGenerator());
                           builder.SetProvider((Dog d) => d.BestDogFriend, new DogFriendInjector());
                           builder.SetProvider((Cat c) => c.Name, new FuncProvider<string>((random) => "cat name..."));
                         }
                     };

        ValueProvider = TestDataGeneratorFactory.CreateValueProvider(domain, false);
        
        for (int i = 0; i < 100; i++)
        {
          Console.WriteLine(ValueProvider.Create<string>());
        }
      };

      It sets_correctString =
          () => ValueProvider.Create<string>().Length.ShouldBeGreaterThan(0);

      static CompoundValueProvider ValueProvider;
    }
    class when_using_TestDataGenerator_simple
    {
      Because of = () =>
      {
        var simpleDomain = new DomainConfiguration
                           {
                               Rules = new RuleSet(new RuleInfo<string>(new StringMarryRule(), 1f, 0.05f, 10))
                           };

        var testDataGenerator = TestDataGeneratorFactory.CreateDataGenerator(simpleDomain);

        var initialDataProvider = testDataGenerator.InitialDataProvider;
        for (var i = 0; i < 1000; i++)
        {
          initialDataProvider.Add("some sexy string " + i);
        }

        var initialData = initialDataProvider.Build();
        var resultData = testDataGenerator.Generate(1, initialData);

        var resultStrings = resultData.GetResult<string>();

        var count = resultStrings.Count(result => result.Contains("Marr"));
        Console.WriteLine("Married ppl: " + count);
      };

      It doesNothing = () => true.ShouldBeTrue();
    }
  }

  #region Performance
  internal class Universe
  {
    public string Name { get; set; }
    public float SizeInLightYears { get; set; }
    public int PeopleCount { get; set; }

    public StarSystem StarSystem { get; set; }  //TODO declare as List as soon as available
  }

  internal class StarSystem
  {
    public string Name { get; set; }
    public float SizeInLightYears { get;set; }
    public int PeopleCount { get;set; }

    public Planet Planet { get; set; } //TODO declare as List as soon as available
  }

  internal class Planet
  {
    public string Name { get;set; }
    public float SizeInKilometers { get; set; }
    public int PeopleCount { get; set; }
    public PlanetColor Color { get; set; }
  }

  internal enum PlanetColor
  {
    Blue, Red
  }

  #endregion

  internal class when_using_TestDataGenerator_complex
  {
    Because of = () =>
    {
      var lifeRuleSet = new RuleSet(new RuleInfo<Person>(new ProcreationRule(), 1f, 0.05f, 1000000)); //TODO: RuleInfo is not so good - implement this directly on the rule
      lifeRuleSet.AddGlobalRule(new AgingRule());

      var complexDomain = new DomainConfiguration //TODO: Configuration
                          {
                            Rules = lifeRuleSet,
                            BuildValueProvider = builder=>
                              {
                                 builder.SetProvider(new GenderGenerator());
                              }
                          };

      var testDataGenerator = TestDataGeneratorFactory.CreateDataGenerator(complexDomain); //TODO: meaningful name... facade to factory .. remove factory

      var initialDataProvider = testDataGenerator.InitialDataProvider;
      initialDataProvider.Add(new Person("Adam", Gender.Male));
      initialDataProvider.Add(new Person("Eve", Gender.Female));

      var initialData = initialDataProvider.Build();
      var resultData = testDataGenerator.Generate(100, initialData);

      var resultPersons = resultData.GetResult<Person>();

      var count = resultPersons.Count();
      Console.WriteLine("ppl count: " + count);
    };

    It does_nothing = () => true.ShouldBeTrue();
  }
}

internal class GenderGenerator : ValueProvider<Gender>
{
  protected override Gender GetValue ()
  {
    return (Gender) Random.Next(0, 2);
  }
}

//TODO: Rule for every generation... not per type
//TODO: World class that contains global generation data - like World.IsFertile..
internal class AgingRule : GlobalRule<Person>
{
  protected override void Execute (Handle<Person> person)
  {
    person.UserData.IsPregnant = false;
    person.Value.Age++;
  }
}

internal class ProcreationRule : Rule<Person>
{
  //TODO: Rule Appliance Probability based on World.Fertility...

  public override IEnumerable<IRuleParameter> GetRuleInputs ()
  {
    yield return new RuleParameter<Person>(p => p.Value.Age >= 14 && p.Value.Gender == Gender.Male);
    yield return
        new RuleParameter<Person>(p => p.Value.Age >= 14 && p.Value.Gender == Gender.Female && (p.UserData.IsPregnant == null || !p.UserData.IsPregnant));
  }

  //TODO: dataProvider -rename or better: yield return..  and use in next generation (not for next rules..)
  public override void Execute (List<IRuleInput> inputData, GeneratorDataProvider dataProvider, CompoundValueProvider valueProvider)
  {
    var male = inputData[0].GetValue<Person>();//TODO: replace list with custom type?
    var female = inputData[1].GetValue<Person>();

    var childCount = valueProvider.Random.Next(1, 1);
    for(var i=0;i<childCount;i++)
    {
      var child = valueProvider.Create<Person>(2);
      child.Father = male.Value;
      child.Mother = female.Value;

      dataProvider.Add(child);
    }

    female.UserData.IsPregnant = true;
  }
}

class Person
{
  public string Name { get; set; }
  public int Age { get; set; }
  public Gender Gender { get; set; }

  public Person Father { get; set; }
  public Person Mother { get; set; }

  public Person()
  {

  }

  public Person (string name, Gender gender, int age=0)
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

#region Simple

public class StringGenerator : ValueProvider<string>
{
  protected override string GetValue ()
  {
    return "Some random string...";
  }
}

public class StringMarryRule:Rule<string>
  {
    public override IEnumerable<IRuleParameter> GetRuleInputs ()
    {
      Func<Handle<string>, bool> predicate = p => p.Value.Length > 3 && (p.UserData.IsMarried == null || !p.UserData.IsMarried);
      yield return new RuleParameter<string>(predicate);
      yield return new RuleParameter<string>(predicate); //TODO: how to define excludes on rule filter basis?
    }

    public override void Execute(List<IRuleInput> inputData, GeneratorDataProvider dataProvider, CompoundValueProvider valueProvider) //e.g. one instance - stores all generation data..
    {
      var sexyString1 = inputData[0].GetValue<string>();
      var sexyString2 = inputData[1].GetValue<string>();

      sexyString1.Value += "[Married to:" + sexyString2.Value + "]";
      sexyString2.Value += "[Married to:" + sexyString1.Value + "]";

      sexyString1.UserData.IsMarried = true;
      sexyString2.UserData.IsMarried = true;
    }
  }

  class Cat
  {
    public string Name { get; set; }
  }

  class Dog
  {
    public string FirstName { get; [UsedImplicitly] set; }
    public string LastName { get; [UsedImplicitly] set; }
    public int Age { get; [UsedImplicitly] set; }

    public Cat BestCatFriend { get; [UsedImplicitly] set; }
    public Dog BestDogFriend { get; [UsedImplicitly] set; }
  }

  class DogFriend : Dog
  {
  }

  class CatGenerator:ValueProvider<Cat>
  {
    protected override Cat GetValue ()
    {
      return new Cat { Name = "Nice cat" };
    }
  }

  class DogFriendInjector:ValueProvider<Dog>
  {
    protected override Dog GetValue ()
    {
      return new DogFriend();
    }
  }

  class DogNameGenerator:ValueProvider<string>
  {
    private readonly string _additionalContent;

    public DogNameGenerator (string additionalContent)
    {
      _additionalContent = additionalContent;
    }

    protected override string GetValue ()
    {
      return "Dog Name String Gen - " + _additionalContent;
    }
  }
#endregion

