using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.SampleCode.Classes
{
  internal class Dog
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }

    public Cat BestCatFriend { get; set; }
    public Dog BestDogFriend { get; set; }
  }
}