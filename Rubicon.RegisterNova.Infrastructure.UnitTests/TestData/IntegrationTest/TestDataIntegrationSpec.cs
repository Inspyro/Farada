using System;
using Machine.Specifications;
using Rubicon.RegisterNova.Infrastructure.JetBrainsAnnotations;
using Rubicon.RegisterNova.Infrastructure.TestData;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode;
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
        var randomGeneratorProvider = new RandomGeneratorProvider();

        randomGeneratorProvider.SetBase(new StringGenerator());
        randomGeneratorProvider.Add(new DogNameStringGenerator());

        var testDataGeneratorFactory = new TestDataGeneratorFactory(randomGeneratorProvider);

        var valueProviderBuilder = testDataGeneratorFactory.ValueProviderBuilderFactory.GetDefault();
        valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("first name"), d => d.FirstName);
        valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("last name"), d => d.LastName);
        valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("dog friend first name"), d => d.BestDogFriend.FirstName);
        valueProviderBuilder.SetProvider(new CatGenerator());

        valueProviderBuilder.SetProvider<Dog, Dog>(new DogFriendInjector(), d => d.BestDogFriend);
        valueProviderBuilder.SetProvider<string, Cat>(new FuncProvider<string>(() => "cat name..."), c => c.Name);

        var testDataGenerator = testDataGeneratorFactory.Build(valueProviderBuilder);
        ValueProvider = testDataGenerator.ValueProvider;
      };

      It sets_correctString =
          () => ValueProvider.Get<string>().ShouldEqual("some String...");

      It sets_correctDogFirstName = () => ValueProvider.Get<Dog>().FirstName.ShouldEqual("Dog Name String Gen - first name");

      It sets_correctDogLastName = () => ValueProvider.Get<Dog>().LastName.ShouldEqual("Dog Name String Gen - last name");

      It returns_correctDogType = () => ValueProvider.Get<Dog>().GetType().ShouldEqual(typeof (Dog));

      It sets_correctBestFriendDogFirstName = () => ValueProvider.Get<Dog>().BestDogFriend.FirstName.ShouldEqual("Dog Name String Gen - dog friend first name");

      It sets_correctBestFriendDogLastName = () => ValueProvider.Get<Dog>().BestDogFriend.LastName.ShouldEqual("Dog Name String Gen - last name");

      It returns_correctBestFriendDogType = () => ValueProvider.Get<Dog>().BestDogFriend.GetType().ShouldEqual(typeof (DogFriend));

      It sets_correctDogAge = () => ValueProvider.Get<Dog>().Age.ShouldEqual(0);

      It sets_correctBestCatFriendsName = () => ValueProvider.Get<Dog>().BestCatFriend.Name.ShouldEqual("cat name...");

      It sets_correctCatWithoutProperties = () => ValueProvider.Get<Cat>(0).Name.ShouldEqual("Nice cat");

      It sets_correctInt = () => ValueProvider.Get<int>().ShouldEqual(0);

      static TypeValueProvider ValueProvider;
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
}
