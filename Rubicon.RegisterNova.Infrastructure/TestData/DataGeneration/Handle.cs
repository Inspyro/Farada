using System;
using System.Dynamic;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public class Handle<T>
  {
    private readonly Action _deleteAction;
    private T _value;
    private bool _isDeleted;

    internal Handle(T value, Action deleteAction)
    {
      ArgumentUtility.CheckNotNull("value", value);
      ArgumentUtility.CheckNotNull("deleteAction", deleteAction);

      Value = value;
      _deleteAction = deleteAction;
      UserData = new UserData();
    }

    public T Value
    {
      get
      {
        if (_isDeleted)
          throw new InvalidOperationException("You cannot get the value on a deleted handle");

        return _value;
      }
      set
      {
        if (_isDeleted)
          throw new InvalidOperationException("You cannot set the value on a deleted handle");

        _value = value;
      }
    }

    public dynamic UserData { get; private set; }

    public void Delete()
    {
      if (_isDeleted)
        throw new InvalidOperationException("The handle was already deleted");

      _deleteAction();
      _isDeleted = true;
    }
  }
}