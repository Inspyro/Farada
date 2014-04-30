using System;
using Rubicon.RegisterNova.Infrastructure.TestData.SampleCode.Classes;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.SampleCode.ValueProviders
{
  internal class DogFriendInjector:ValueProvider<Dog>
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
}