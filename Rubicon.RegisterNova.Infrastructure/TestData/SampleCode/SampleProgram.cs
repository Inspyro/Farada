using System;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode;
using Rubicon.RegisterNova.Infrastructure.TestData.SampleCode.Classes;
using Rubicon.RegisterNova.Infrastructure.TestData.SampleCode.ValueProviders;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;

namespace Rubicon.RegisterNova.Infrastructure.TestData.SampleCode
{
  static class SampleProgram
  {
    public static void Main ()
    {
      var valueProviderBuilder = ChainValueProviderBuilderFactory.GetDefault();
      valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("first name"), d => d.FirstName);
      valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("last name"), d => d.LastName);
      valueProviderBuilder.SetProvider<string, Dog>(new DogNameGenerator("dog friend first name"), d => d.BestDogFriend.FirstName);
      valueProviderBuilder.SetProvider(new CatGenerator());

      valueProviderBuilder.SetProvider<Dog, Dog>(new DogFriendInjector(), d => d.BestDogFriend);
      valueProviderBuilder.SetProvider<string, Cat>(new FuncProvider<string>(() => "cat name..."), c => c.Name);

      var typeValueProvider = new TypeValueProvider(valueProviderBuilder.ToValueProvider());

      var someString=typeValueProvider.Get<string>();
      Console.WriteLine(someString);

      var dog = typeValueProvider.Get<Dog>();
      Console.WriteLine(dog.FirstName);
      Console.WriteLine(dog.LastName);
      Console.WriteLine("dog concrete type: " + dog.GetType());
      Console.WriteLine("BestDogFriend - First:" + dog.BestDogFriend.FirstName);
      Console.WriteLine("BestDogFriend - Last:" + dog.BestDogFriend.LastName);
      Console.WriteLine("BestDogFriend concrete type: " + dog.BestDogFriend.GetType());
      Console.WriteLine("DogAge:"+dog.Age);
      Console.WriteLine("BestCatFriendName:" + dog.BestCatFriend.Name);

      var cat = typeValueProvider.Get<Cat>(0); //0 deptht means the properties are not filled
      Console.WriteLine("Cat name:"+cat.Name);

      var someInt = typeValueProvider.Get<int>();
      Console.WriteLine(someInt);

      Console.ReadKey();
    }
  }
}