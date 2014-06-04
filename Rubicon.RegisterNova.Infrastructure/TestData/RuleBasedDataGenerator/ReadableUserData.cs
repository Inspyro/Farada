using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Rubicon.RegisterNova.Infrastructure.TestData.RuleBasedDataGenerator
{
  public class ReadableUserData : DynamicObject
  {
    protected readonly Dictionary<string, object> Data;

    public ReadableUserData()
    {
       Data= new Dictionary<string, object>();
    }

    public ReadableUserData(ReadableUserData clone)
    {
      Data=new Dictionary<string, object>(clone.Data);
    }

    public object this [string name]
    {
      get { return Data[name]; }
    }

    public override bool TryGetMember (GetMemberBinder binder, out object result)
    {
      Data.TryGetValue(binder.Name, out result);
      return true;
    }

    public override bool TrySetMember (SetMemberBinder binder, object value)
    {
      throw new InvalidOperationException("you cannot set values on this object as it is readonly..");
    }
  }
}