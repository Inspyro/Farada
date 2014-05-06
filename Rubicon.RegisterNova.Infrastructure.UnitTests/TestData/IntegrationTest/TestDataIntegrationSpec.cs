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
  internal class TestDataIntegrationSpec
  {
    class when_using_TypeValueProvider
    {
      Because of = () =>
      {
        var randomGeneratorProvider = new RandomGeneratorProviderFactory(new Random()).GetDefault();

        randomGeneratorProvider.SetBase(new StringGenerator());
        randomGeneratorProvider.Add(new DogNameStringGenerator());

        var testDataGeneratorFactory = new TestDataGeneratorFactory(randomGeneratorProvider);

        var valueProviderBuilder = testDataGeneratorFactory.ValueProviderBuilderFactory.GetDefault();
        valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("first name"), d => d.FirstName);
        valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("last name"), d => d.LastName);
        valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("dog friend first name"), d => d.BestDogFriend.FirstName);
        valueProviderBuilder.SetProvider(new CatGenerator());

        valueProviderBuilder.SetProvider<Dog, Dog>(new DogFriendInjector(), d => d.BestDogFriend);
        valueProviderBuilder.SetProvider<string, Cat>(new FuncProvider<string>((randomGenerator) => "cat name..."), c => c.Name);

        var testDataGenerator = testDataGeneratorFactory.Build(valueProviderBuilder);
        ValueProvider = testDataGenerator.ValueProvider;
      };

      It sets_correctString =
          () => ValueProvider.Get<string>().ShouldEqual("Some random string...");

      It sets_correctDogFirstName = () => ValueProvider.Get<Dog>().FirstName.ShouldEqual("Dog Name String Gen - first name");

      It sets_correctDogLastName = () => ValueProvider.Get<Dog>().LastName.ShouldEqual("Dog Name String Gen - last name");

      It returns_correctDogType = () => ValueProvider.Get<Dog>().GetType().ShouldEqual(typeof (Dog));

      It sets_correctBestFriendDogFirstName =
          () => ValueProvider.Get<Dog>().BestDogFriend.FirstName.ShouldEqual("Dog Name String Gen - dog friend first name");

      It sets_correctBestFriendDogLastName = () => ValueProvider.Get<Dog>().BestDogFriend.LastName.ShouldEqual("Dog Name String Gen - last name");

      It returns_correctBestFriendDogType = () => ValueProvider.Get<Dog>().BestDogFriend.GetType().ShouldEqual(typeof (DogFriend));

      It sets_correctDogAge = () => ValueProvider.Get<Dog>().Age.ShouldEqual(0);

      It sets_correctBestCatFriendsName = () => ValueProvider.Get<Dog>().BestCatFriend.Name.ShouldEqual("cat name...");

      It sets_correctCatWithoutProperties = () => ValueProvider.Get<Cat>(0).Name.ShouldEqual("Nice cat");

      It sets_correctInt = () => ValueProvider.Get<int>().ShouldEqual(0);

      static TypeValueProvider ValueProvider;
    }

    class when_using_TypeValueProvider_Word
    {
      Because of = () =>
      {
        var randomGeneratorProvider = new RandomGeneratorProviderFactory(new Random()).GetEmpty();

        randomGeneratorProvider.SetBase(new RandomStringGenerator());
        randomGeneratorProvider.Add(new RandomWordGenerator());

        var testDataGeneratorFactory = new TestDataGeneratorFactory(randomGeneratorProvider);

        var valueProviderBuilder = testDataGeneratorFactory.ValueProviderBuilderFactory.GetEmpty();
        valueProviderBuilder.SetProvider(new WordValueProvider());

        valueProviderBuilder.SetProvider<Dog, Dog>(new DogFriendInjector(), d => d.BestDogFriend);
        valueProviderBuilder.SetProvider<string, Cat>(new FuncProvider<string>((randomGenerator) => "cat name..."), c => c.Name);

        var testDataGenerator = testDataGeneratorFactory.Build(valueProviderBuilder);
        ValueProvider = testDataGenerator.ValueProvider;

        for (int i = 0; i < 100; i++)
        {
          Console.WriteLine(ValueProvider.Get<string>());
        }
      };

      It sets_correctString =
          () => ValueProvider.Get<string>().Length.ShouldBeGreaterThan(0);

      static TypeValueProvider ValueProvider;
    }


    class when_using_TestDataGenerator_simple
    {
      Because of = () =>
      {
        var basicRuleSet = new RuleSet(new RuleInfo<string>(new StringMarryRule(), 1f, 0.05f, 10));

        var testDataGeneratorFactory = new TestDataGeneratorFactory(new RandomGeneratorProviderFactory(new Random()).GetDefault(), basicRuleSet);
        var testDataGenerator = testDataGeneratorFactory.Build(testDataGeneratorFactory.ValueProviderBuilderFactory.GetDefault());

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

    class when_using_TestDataGenerator_simpleFacade
    {
      Because of = () =>
      {
        var simpleDomain = new DataDomain
                           {
                               Rules = new RuleSet(new RuleInfo<string>(new StringMarryRule(), 1f, 0.05f, 10))
                           };

        var testDataGenerator = TestDataGeneratorFacade.Get(simpleDomain);

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

  internal class when_using_TestDataGenerator_complex
  {
    Because of = () =>
    {
      var randomGeneratorProvider = new RandomGeneratorProviderFactory(new Random()).GetDefault();
      randomGeneratorProvider.SetBase(new GenderGenerator());

      var lifeRuleSet = new RuleSet(new RuleInfo<Person>(new ProcreationRule(), 1f, 0.05f, 1000000));
      lifeRuleSet.AddGlobalRule(new AgingRule());

      var testDataGeneratorFactory = new TestDataGeneratorFactory(randomGeneratorProvider, lifeRuleSet);
      var valueProviderBuilder = testDataGeneratorFactory.ValueProviderBuilderFactory.GetDefault();

      valueProviderBuilder.SetProvider(new FuncProvider<Gender>((randomGenerator) => randomGenerator.Next()));

      var testDataGenerator = testDataGeneratorFactory.Build(valueProviderBuilder);


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

  internal class when_using_TestDataGenerator_complexFacade
  {
    Because of = () =>
    {
      var lifeRuleSet = new RuleSet(new RuleInfo<Person>(new ProcreationRule(), 1f, 0.05f, 1000000));
      lifeRuleSet.AddGlobalRule(new AgingRule());

      var complexDomain = new DataDomain
                          {
                            Rules = lifeRuleSet,
                            SetupRandomProviderAction = provider =>
                            {
                              provider.SetBase(new GenderGenerator());
                            },
                            SetupValueProviderAction = provider=>
                              {
                                 provider.SetProvider(new FuncProvider<Gender>((randomGenerator) => randomGenerator.Next()));
                              }
                          };

      var testDataGenerator = TestDataGeneratorFacade.Get(complexDomain);

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

internal class GenderGenerator : RandomGenerator<Gender>
{
  public override Gender Next ()
  {
    return (Gender) Random.Next(0, 2);
  }
}

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
  public override IEnumerable<IRuleParameter> GetRuleInputs ()
  {
    yield return new RuleParameter<Person>(p => p.Value.Age >= 14 && p.Value.Gender == Gender.Male);
    yield return
        new RuleParameter<Person>(p => p.Value.Age >= 14 && p.Value.Gender == Gender.Female && (p.UserData.IsPregnant == null || !p.UserData.IsPregnant));
  }

  public override void Execute (List<IRuleInput> inputData, GeneratorDataProvider dataProvider, TypeValueProvider valueProvider)
  {
    var male = inputData[0].GetValue<Person>();
    var female = inputData[1].GetValue<Person>();

    var childCount = valueProvider.Random.Next(1, 1);
    for(var i=0;i<childCount;i++)
    {
      var child = valueProvider.Get<Person>(2);
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

public class StringGenerator : RandomGenerator<string>
{
  public override string Next ()
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

    public override void Execute(List<IRuleInput> inputData, GeneratorDataProvider dataProvider, TypeValueProvider valueProvider) //e.g. one instance - stores all generation data..
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
    public CatGenerator (ValueProvider<Cat> nextProvider=null)
        : base(nextProvider)
    {
    }

    protected override Cat GetValue (Cat currentValue)
    {
      return new Cat { Name = "Nice cat" };
    }
  }

  class CustomStringGenerator : ValueProvider<string>
  {
    public CustomStringGenerator (ValueProvider<string> nextProvider=null)
        : base(nextProvider)
    {
    }

    protected override string GetValue (string currentValue)
    {
      return currentValue.Substring(1, 2);
    }
  }

  class DogFriendInjector:ValueProvider<Dog>
  {
    public DogFriendInjector (ValueProvider<Dog> nextProvider=null)
        : base(nextProvider)
    {
    }

    protected override Dog GetValue (Dog currentValue)
    {
      return new DogFriend();
    }
  }

  class DogNameGenerator:ValueProvider<string, DogNameStringGenerator>
  {
    private readonly string _additionalContent;

    public DogNameGenerator (string additionalContent, ValueProvider<string> nextProvider=null)
        : base(nextProvider)
    {
      _additionalContent = additionalContent;
    }

    protected override string GetValue (string currentValue)
    {
      return RandomGenerator.Next() +" - "+ _additionalContent;
    }
  }

  internal class DogNameStringGenerator:RandomGenerator<string>
  {
    public override string Next ()
    {
      return "Dog Name String Gen";
    }
  }
#endregion

