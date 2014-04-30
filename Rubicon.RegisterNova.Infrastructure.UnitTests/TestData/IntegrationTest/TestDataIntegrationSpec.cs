using System;
using Machine.Specifications;
using Rubicon.RegisterNova.Infrastructure.JetBrainsAnnotations;
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
        var valueProviderBuilder = ChainValueProviderBuilderFactory.GetDefault();
        valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("first name"), d => d.FirstName);
        valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("last name"), d => d.LastName);
        valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("dog friend first name"), d => d.BestDogFriend.FirstName);
        valueProviderBuilder.SetProvider(new CatGenerator());

        valueProviderBuilder.SetProvider<Dog, Dog>(new DogFriendInjector(), d => d.BestDogFriend);
        valueProviderBuilder.SetProvider<string, Cat>(new FuncProvider<string>(() => "cat name..."), c => c.Name);

        TypeValueProvider = new TypeValueProvider(valueProviderBuilder.ToValueProvider());
      };

      It sets_correctString =
          () => TypeValueProvider.Get<string>().ShouldEqual("some String...");

      It sets_correctDogFirstName = () => TypeValueProvider.Get<Dog>().FirstName.ShouldEqual("I am a dog - first name");

      It sets_correctDogLastName = () => TypeValueProvider.Get<Dog>().LastName.ShouldEqual("I am a dog - last name");

      It returns_correctDogType = () => TypeValueProvider.Get<Dog>().GetType().ShouldEqual(typeof (Dog));

      It sets_correctBestFriendDogFirstName = () => TypeValueProvider.Get<Dog>().BestDogFriend.FirstName.ShouldEqual("I am a dog - dog friend first name");

      It sets_correctBestFriendDogLastName = () => TypeValueProvider.Get<Dog>().BestDogFriend.LastName.ShouldEqual("I am a dog - last name");

      It returns_correctBestFriendDogType = () => TypeValueProvider.Get<Dog>().BestDogFriend.GetType().ShouldEqual(typeof (DogFriend));

      It sets_correctDogAge = () => TypeValueProvider.Get<Dog>().Age.ShouldEqual(0);

      It sets_correctBestCatFriendsName = () => TypeValueProvider.Get<Dog>().BestCatFriend.Name.ShouldEqual("cat name...");

      It sets_correctCatWithoutProperties = () => TypeValueProvider.Get<Cat>(0).Name.ShouldEqual("Nice cat");

      It sets_correctInt = () => TypeValueProvider.Get<int>().ShouldEqual(0);

      static TypeValueProvider TypeValueProvider;
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
      return base.GetValue(currentValue.Substring(1,2));
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

  class DogNameGenerator:ValueProvider<string>
  {
    private readonly string _additionalContent;

    public DogNameGenerator (string additionalContent, ValueProvider<string> nextProvider=null)
        : base(nextProvider)
    {
      _additionalContent = additionalContent;
    }

    protected override string GetValue (string currentValue)
    {
      return "I am a dog - " + _additionalContent;
    }
  }
}
