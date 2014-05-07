using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public class RuleValue<T>:IRuleValue
  {
    private Action _deleteAction;
    private bool _isDeleted;

    private T _value;

    public RuleValue(T value)
    {
      _value = value;
      UserData = new UserData();
    }

    public void OnDeleted(Action deleteAction)
    {
      _deleteAction = deleteAction;
    }

    public dynamic UserData { get; private set; }

    public void Delete()
    {
      if (_isDeleted)
        throw new InvalidOperationException("The handle was already deleted");

      _deleteAction();
      _isDeleted = true;
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

    public T1 GetValue<T1> ()
    {
      return (T1) (object) Value;
    }

    public void SetValue<T1>(T1 value)
    {
      Value = (T) (object) value;
    }

    public Type GetDataType ()
    {
      return typeof (T);
    }
  }

  public interface IRuleValue
  {
    void OnDeleted (Action deleteAction);

    dynamic UserData { get; }
    T GetValue<T>();
    void SetValue<T>(T value);
    void Delete ();
    Type GetDataType ();
  }
}