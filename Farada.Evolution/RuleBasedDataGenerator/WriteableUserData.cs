using System;
using System.Dynamic;

namespace Farada.Evolution.RuleBasedDataGenerator
{
  public class WriteableUserData : ReadableUserData
  {
    private bool _isWriteable = true;

    public override bool TrySetMember (SetMemberBinder binder, object value)
    {
      if(!_isWriteable)
      {
        return base.TrySetMember(binder, value);
      }

      Data[binder.Name] = value;
      return true;
    }

    public ReadableUserData MakeReadOnly ()
    {
      return new ReadableUserData(this);
    }
  }
}