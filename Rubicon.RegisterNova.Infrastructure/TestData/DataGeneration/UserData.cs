using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public class UserData : DynamicObject
  {
    private readonly Dictionary<string, object> _data;

    public UserData()
    {
       _data= new Dictionary<string, object>();
    }

    public override bool TryGetMember (GetMemberBinder binder, out object result)
    {
      _data.TryGetValue(binder.Name, out result);
      return true;
    }

    public override bool TrySetMember (SetMemberBinder binder, object value)
    {
      _data[binder.Name] = value;
      return true;
    }
  }
}