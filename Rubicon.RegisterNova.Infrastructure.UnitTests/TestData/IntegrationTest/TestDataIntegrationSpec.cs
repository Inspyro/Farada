using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Machine.Specifications;
using Rubicon.RegisterNova.Infrastructure.JetBrainsAnnotations;
using Rubicon.RegisterNova.Infrastructure.TestData;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.RuleBasedDataGeneration;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders;
using Rubicon.RegisterNova.Infrastructure.TestData.Parallelization;
using Rubicon.RegisterNova.Infrastructure.TestData.RuleBasedDataGenerator;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.UnitTests.TestData.IntegrationTest;
using Rubicon.RegisterNova.Infrastructure.Validation;

namespace Rubicon.RegisterNova.Infrastructure.UnitTests.TestData.IntegrationTest
{
  [Subject (typeof (RuleBasedDataGenerator))]
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
                           builder.AddProvider((string s)=>s, new StringGenerator());
                           builder.AddProvider((int i)=>i, context => 10);

                           builder.AddProvider((Dog d) => d.FirstName, new DogNameGenerator("first name"));
                           builder.AddProvider((Dog d) => d.LastName, new DogNameGenerator("last name"));
                           builder.AddProvider((Dog d) => d.BestDogFriend.FirstName, new DogNameGenerator("friend first name"));
                           builder.AddProvider((Dog d) => d.BestDogFriend.LastName, new LastValueGenerator("friend last name"));

                           builder.AddProvider((Cat c) => c, new CatGenerator());
                           builder.AddProvider((Animal a) => a, new AnimalMouseFiller());
                           builder.AddProvider((Elephant e) => e, new ElephantInjector());

                           builder.AddProvider((Elephant e) => e.DefinedNullable, context => 100);

                           builder.AddProvider((Dog d) => d.BestDogFriend, new DogFriendInjector());
                           builder.AddProvider((Cat c) => c.Name, ctx => "cat name...");

                           builder.AddProvider((string s, SomeValue sv)=>s, new SomeAttributeBasedFiller());
                           builder.AddProvider((string s, DogValue dv) => s, new DogAttributeBasedFiller());

                           builder.AddProvider((Animal a) => a.HomePlanet, ctx => "Earth");
                         }
                     };

        ValueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider(domain);
      };

      It sets_correctString =
        () => 
          ValueProvider.Create<string>().ShouldEqual("default string");

      It sets_correctNullableValue =
          () =>
              ValueProvider.Create<int?>().Value.ShouldEqual(10);

      It sets_correctDogFirstName = () =>
          ValueProvider.Create<Dog>().FirstName.ShouldEqual("Dog_first name");

      It sets_correctDogLastName = () => ValueProvider.Create<Dog>().LastName.ShouldEqual("Dog_last name");

      It sets_correctDogDefault =
          () =>
            ValueProvider.Create<Dog>().Default.ShouldEqual("default string");

      It sets_correctNonAttributeValue = () => 
        ValueProvider.Create<Dog>().AttributedValue.ShouldEqual("default string_test dog attribute");


      It sets_correctAttributeValue =
          () => ValueProvider.Create<Dog>().BestDogFriend.AttributedValue.ShouldEqual("default string_test dog attribute");


      It returns_correctDogType = () => ValueProvider.Create<Dog>().GetType().ShouldEqual(typeof (Dog));

      It sets_correctBestFriendDogFirstName =
          () =>
              ValueProvider.Create<Dog>().BestDogFriend.FirstName.ShouldEqual("Dog_friend first name");

      It sets_correctBestFriendDogLastName =
          () =>
              ValueProvider.Create<Dog>().BestDogFriend.LastName.ShouldEqual("Dog_last name_friend last name");

      It returns_correctBestFriendDogType = () => 
        ValueProvider.Create<Dog>().BestDogFriend.GetType().ShouldEqual(typeof (DogFriend));

      It sets_correctDogAge = () => ValueProvider.Create<Dog>().Age.ShouldEqual(10);

      It sets_correctBestCatFriendsName = () =>
          ValueProvider.Create<Dog>().BestCatFriend.Name.ShouldEqual("cat name...");

      It sets_correctCatWithoutProperties = () => 
        ValueProvider.Create<Cat>(0).Name.ShouldEqual("Nice cat");

      It sets_correctInt = () => ValueProvider.Create<int>().ShouldEqual(10);

      It sets_dogHomePlanet = () => ValueProvider.Create<Dog>().HomePlanet.ShouldEqual("Earth");
      It sets_catHomePlanet = () => ValueProvider.Create<Cat>().HomePlanet.ShouldEqual("Earth");

      It sets_correctAnimal = () => ValueProvider.Create<Zoo>().SomeAnimal.GetType().ShouldEqual(typeof (Mouse));
      It sets_correctMouse = () => ValueProvider.Create<Zoo>().Mouse.GetType().ShouldEqual(typeof (Mouse));
      It sets_correctElephant = () => ValueProvider.Create<Zoo>().Elephant.GetType().ShouldEqual(typeof (Elephant));

      
      It sets_DefaultValueOfElephant = () => ValueProvider.Create<Elephant>().DefaultNullable.Value.ShouldEqual(10);
      It sets_DefinedValueOfElephant = () => ValueProvider.Create<Elephant>().DefinedNullable.Value.ShouldEqual(100);

      static ICompoundValueProvider ValueProvider;
    }

    class when_using_TypeValueProvider_WithNullModifier
    {
      Because of = () =>
      {
        var domain = new BaseDomainConfiguration
                     {
                         BuildValueProvider = builder => { builder.AddInstanceModifier(new NullModifier(1)); }
                     };

        ValueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider(domain);
      };

      It generates_correctTestClass = () =>
      {
        var tc = ValueProvider.Create<TestClass>();
        tc.NeverNullProp.ShouldNotBeNull();
        tc.RequiredPrpo.ShouldNotBeNull();
        tc.OtherProp.ShouldBeNull();
      };

      static ICompoundValueProvider ValueProvider;
    }

     class when_using_TypeValueProvider_WithMinMaxConstraints
    {
      Because of = () =>
      {
        ValueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider(new BaseDomainConfiguration());
      };

      It generates_correctTestClass = () =>
      {
        var cwc = ValueProvider.Create<ClassWithConstraints>();
        cwc.LongName.Length.ShouldBeGreaterThanOrEqualTo(10000);
        cwc.ShortName.Length.ShouldBeLessThanOrEqualTo(1);

        cwc.MediumName.Length.ShouldBeGreaterThanOrEqualTo(10);
        cwc.MediumName.Length.ShouldBeLessThanOrEqualTo(20);
      };

      static ICompoundValueProvider ValueProvider;
    }

    class ClassWithConstraints
    {
      [MinLength (10000)]
      public string LongName { get; set; }

      [MaxLength (1)]
      public string ShortName { get; set; }

      [MinLength (10)]
      [MaxLength (20)]
      public string MediumName { get; set; }
    }

    class TestClass
    {
      [Required]
      public string RequiredPrpo { get; set; }

      [NeverNull]
      public string NeverNullProp { get; set; }

      public string OtherProp { get; set; }
    }

    class when_using_TypeValueProvider_DefaultTypes
    {
      Because of = () =>
      {
        var valueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider(new BaseDomainConfiguration());

        for (var i = 0; i < 100; i++)
        {
          Console.WriteLine(valueProvider.Create<bool>());
          Console.WriteLine(valueProvider.Create<byte>());
          Console.WriteLine(valueProvider.Create<char>());
          Console.WriteLine(valueProvider.Create<decimal>());
          Console.WriteLine(valueProvider.Create<double>());
          Console.WriteLine(valueProvider.Create<SomeEnum>());
          Console.WriteLine(valueProvider.Create<EmptyEnum>());
          Console.WriteLine(valueProvider.Create<float>());
          Console.WriteLine(valueProvider.Create<int>());
          Console.WriteLine(valueProvider.Create<long>());
          Console.WriteLine(valueProvider.Create<sbyte>());
          Console.WriteLine(valueProvider.Create<short>());
          Console.WriteLine(valueProvider.Create<uint>());
          Console.WriteLine(valueProvider.Create<ulong>());
          Console.WriteLine(valueProvider.Create<ushort>());

          Console.WriteLine(valueProvider.Create<string>());
          Console.WriteLine(valueProvider.Create<DateTime>());
        }
      };

      enum EmptyEnum
      {
      }

      enum SomeEnum
      {
        Value1,
        Value2
      }

      It doesNothing = () => true.ShouldBeTrue();
    }

    class when_using_TypeValueProvider_Performance
    {
      Because of = () =>
      {
        var basicDomain = new DomainConfiguration();
        var valueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider(basicDomain);

        const int count = 4000000; //1 million

        var start = DateTime.Now;

        var result = valueProvider.CreateMany<Universe>(count);

        Console.WriteLine("Took {0} s to generate {1} universes", (DateTime.Now - start).TotalSeconds, result.Count());
      };

      It doesNothing = () => true.ShouldBeTrue();
    }

    class when_using_TypeValueProvider_ThreadedPerformance
    {
      Because of = () =>
      {
        var basicDomain = new DomainConfiguration();
        var valueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider(basicDomain);

        const int count = 1000000; //1 million

        var start = DateTime.Now;
        var listOfUniverses = Parallelization.DistributeParallel(chunkCount => valueProvider.CreateMany<Universe>(chunkCount), count).ToList();

        Console.WriteLine(
            "Took {0} s to generate {1} universes",
            (DateTime.Now - start).TotalSeconds,listOfUniverses.Count);
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
                           builder.AddProvider((string s) => s, new RandomWordGenerator(new RandomSyllabileGenerator()));
                           builder.AddProvider((Dog d) => d.BestDogFriend, new DogFriendInjector());
                           builder.AddProvider((Cat c) => c.Name, ctx => "cat name...");
                         }
                     };

        ValueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider(domain, false);

        for (int i = 0; i < 100; i++)
        {
          Console.WriteLine(ValueProvider.Create<string>());
        }
      };

      It sets_correctString =
          () => ValueProvider.Create<string>().Length.ShouldBeGreaterThan(0);

      static ICompoundValueProvider ValueProvider;
    }

    class when_using_TestDataGenerator_simple
    {
      Because of = () =>
      {
        var simpleDomain = new DomainConfiguration
                           {
                               Rules = new RuleSet(new StringMarryRule())
                           };

        var testDataGenerator = TestDataGeneratorFactory.CreateRuleBasedDataGenerator(simpleDomain);

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

  internal class UniverseList
  {
    public IReadOnlyList<Universe> InternalList { get; set; }
  }

  internal class ElephantInjector : ValueProvider<Elephant>
  {
    protected override Elephant CreateValue (ValueProviderContext<Elephant> context)
    {
      return new Elephant();
    }
  }

  internal class AnimalMouseFiller : ValueProvider<Animal>
  {
    protected override Animal CreateValue (ValueProviderContext<Animal> context)
    {
      return new Mouse();
    }
  }

  internal class Animal
  {
    public string HomePlanet { get; [UsedImplicitly] set; }
  }

  internal class Zoo
  {
    public Animal SomeAnimal { get; [UsedImplicitly] set; }

    public Elephant Elephant { get; [UsedImplicitly] set; }
    public Mouse Mouse { get; [UsedImplicitly] set; }
  }

  internal class Mouse:Animal
  {
    public string Name { get; [UsedImplicitly]set; }
  }
  
  internal class Elephant:Animal
  {
    public string Name { get; [UsedImplicitly]set; }

    public int? DefaultNullable { get; [UsedImplicitly] set; }
    public int? DefinedNullable { get; [UsedImplicitly] set; }
  }

  internal class SomeAttributeBasedFiller : AttributeBasedValueProvider<string, SomeValue>
  {
    protected override string CreateValue (AttributeValueProviderContext<string, SomeValue> context)
    {
      return context.GetPreviousValue() + "_" + context.Attribute.Text;
    }
  }

  internal class SomeValue : Attribute
  {
    public string Text { get; private set; }

    public SomeValue (string text)
    {
      Text = text;
    }
  }

  internal class DogAttributeBasedFiller : AttributeBasedValueProvider<string, DogValue>
  {
    protected override string CreateValue (AttributeValueProviderContext<string, DogValue> context)
    {
      return context.GetPreviousValue() + "_" + context.Attribute.Text;
    }
  }

  internal class DogValue : Attribute
  {
    public string Text { get; private set; }

    public DogValue (string text)
    {
      Text = text;
    }
  }

  internal class LastValueGenerator : ValueProvider<string>
  {
    readonly string _additionalValue;

    public LastValueGenerator (string additionalValue)
    {
      _additionalValue = additionalValue;
    }

    protected override string CreateValue (ValueProviderContext<string> context)
    {
      return context.GetPreviousValue() + "_" + _additionalValue;
    }
  }

  #region Performance

  internal class Universe
  {
    public string Name { get; set; }
    public float SizeInLightYears { get; set; }
    public int PeopleCount { get; set; }

    public StarSystem StarSystem { get; set; } //TODO declare as List as soon as available
  }

  internal class StarSystem
  {
    public string Name { get; set; }
    public float SizeInLightYears { get; set; }
    public int PeopleCount { get; set; }

    public Planet Planet { get; set; } //TODO declare as List as soon as available
  }

  internal class Planet
  {
    public string Name { get; set; }
    public float SizeInKilometers { get; set; }
    public int PeopleCount { get; set; }
    public PlanetColor Color { get; set; }
  }

  internal enum PlanetColor
  {
    Blue,
    Red
  }

  #endregion

  internal class when_using_TestDataGenerator_complex
  {
    Because of = () =>
    {
      var lifeRuleSet = new RuleSet(new ProcreationRule(), new AgingRule());
      lifeRuleSet.AddGlobalRule(new WorldRule());

      var complexDomain = new DomainConfiguration
                          {
                              Rules = lifeRuleSet,
                              BuildValueProvider = builder => { builder.AddProvider((Gender g)=>g, new GenderGenerator()); }
                          };

      var testDataGenerator = TestDataGeneratorFactory.CreateRuleBasedDataGenerator(complexDomain);

      var initialDataProvider = testDataGenerator.InitialDataProvider;
      initialDataProvider.Add(new Person("Adam", Gender.Male));
      initialDataProvider.Add(new Person("Eve", Gender.Female));

      var initialData = initialDataProvider.Build();
      var resultData = testDataGenerator.Generate(50, initialData);

      var resultPersons = resultData.GetResult<Person>();

      //TODO: specify how many persons we want..
      var count = resultPersons.Count();
      Console.WriteLine("ppl count: " + count);
    };

    It does_nothing = () => true.ShouldBeTrue();
  }

  //TODO: Support special distribution like 50% married people, 20% marry twice...
  internal class WorldRule : GlobalRule
  {
    protected override void Execute ()
    {
    }
  }
}

internal class GenderGenerator : ValueProvider<Gender>
{
  protected override Gender CreateValue (ValueProviderContext<Gender> context)
  {
    return (Gender) context.Random.Next(0, 2);
  }
}

//TODO: Rule for every generation... not per type
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
    return LerpUtility.LerpFromLowToHigh(100000, World.Count<Person>(), 1f, 0.1f);
  }

  protected override IEnumerable<IRuleParameter> GetRuleInputs ()
  {
    //TODO: array? config parameter? - compound could mix multiple parameters (see below)
    //TODO: can we have relations between persons? a likes b,c,d hates f,g..?
    yield return new RuleParameter<Person>(
        p => p.Value.Age >= 14 && p.Value.Gender == Gender.Male);
    yield return
        new RuleParameter<Person>(
            (p) => { return p.Value.Age >= 14 && p.Value.Gender == Gender.Female && (p.UserData.IsPregnant == null || !p.UserData.IsPregnant); });

    //TODO: do we need optional rule paramters?
  }

  protected override IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData)
  {
    var male = inputData.GetValue<Person>(0);
    var female = inputData.GetValue<Person>(1);

    var childCount = 1; // TODO: Get Random object from somewhere ValueProvider.Random.Next(1, 1);
    for (var i = 0; i < childCount; i++)
    {
      var child = ValueProvider.Create<Person>();
      child.Father = male.Value;
      child.Mother = female.Value;

      yield return new RuleValue<Person>(child);
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

#region Simple

public class StringGenerator : ValueProvider<string>
{
  protected override string CreateValue (ValueProviderContext<string> context)
  {
    return "default string";
  }
}

public class StringMarryRule : Rule
{
  public override float GetExecutionProbability ()
  {
    return 0.5f;
  }

  protected override IEnumerable<IRuleParameter> GetRuleInputs ()
  {
    Func<RuleValue<string>, bool> predicate = p => p.Value.Length > 3 && (p.UserData.IsMarried == null || !p.UserData.IsMarried);
    yield return new RuleParameter<string>(predicate);
    yield return new RuleParameter<string>(predicate); //TODO: how to define excludes on rule filter basis?
  }

  protected override IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData) //e.g. one instance - stores all generation data..
  {
    var sexyString1 = inputData.GetValue<string>(0);
    var sexyString2 = inputData.GetValue<string>(1);

    sexyString1.Value += "[Married to:" + sexyString2.Value + "]";
    sexyString2.Value += "[Married to:" + sexyString1.Value + "]";

    sexyString1.UserData.IsMarried = true;
    sexyString2.UserData.IsMarried = true;

    yield break;
  }
}

internal class Cat : Animal
{
  public string Name { get; set; }
}

internal class Dog : Animal
{
  public string FirstName { get; [UsedImplicitly] set; }
  public string LastName { get; [UsedImplicitly] set; }
  public string Default { get; [UsedImplicitly] set; }

  //[SomeValue ("some value")]
  [DogValue ("test dog attribute")]
  public string AttributedValue { get; [UsedImplicitly] set; }

  public int Age { get; [UsedImplicitly] set; }

  public Cat BestCatFriend { get; [UsedImplicitly] set; }
  public Dog BestDogFriend { get; [UsedImplicitly] set; }
}

internal class DogFriend : Dog
{
}

internal class CatGenerator : ValueProvider<Cat>
{
  protected override Cat CreateValue (ValueProviderContext<Cat> context)
  {
    return new Cat { Name = "Nice cat" };
  }
}

internal class DogFriendInjector : ValueProvider<Dog>
{
  protected override Dog CreateValue (ValueProviderContext<Dog> context)
  {
    return new DogFriend();
  }
}

internal class DogNameGenerator : ValueProvider<string>
{
  readonly string _additionalContent;

  public DogNameGenerator (string additionalContent)
  {
    _additionalContent = additionalContent;
  }

  protected override string CreateValue (ValueProviderContext<string> context)
  {
    return "Dog_" + _additionalContent;
  }
}

#endregion