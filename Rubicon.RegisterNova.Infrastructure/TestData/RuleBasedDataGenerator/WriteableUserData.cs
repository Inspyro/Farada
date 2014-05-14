using System;
using System.Dynamic;

namespace Rubicon.RegisterNova.Infrastructure.TestData.RuleBasedDataGenerator
{
  public class WriteableUserData : ReadableUserData
  {
    public override bool TrySetMember (SetMemberBinder binder, object value)
    {
      Data[binder.Name] = value;
      return true;
    }
  }
}