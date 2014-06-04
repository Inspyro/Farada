using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.RuleBasedDataGenerator
{
  internal class World : IWriteableWorld
  {
    private readonly WriteableUserData _userData;

    internal World ()
    {
      _userData = new WriteableUserData();
    }

    public T Read<T> (Func<dynamic, T> readFunc)
    {
      return readFunc(_userData.MakeReadOnly());
    }

    public int Count<T> ()
    {
      return 0; //TODO:
    }

    public void Write (Action<dynamic> writeFunc)
    {
      writeFunc(_userData);
    }
  }

  public interface IWriteableWorld:IReadableWorld
  {
    void Write (Action<dynamic> writeFunc);
  }

  public interface IReadableWorld
  {
    T Read<T> (Func<dynamic, T> readFunc);
    int Count<T> ();
  }
}
