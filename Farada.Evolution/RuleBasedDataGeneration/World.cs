using System;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  internal class World : IWriteableWorld
  {
    private readonly WriteableUserData _userData;
    
    internal GeneratorResult CurrentData { get; set; }

    internal World (GeneratorResult initialData)
    {
      CurrentData = initialData;
      _userData = new WriteableUserData();
    }

    public T Read<T> (Func<dynamic, T> readFunc)
    {
      return readFunc(_userData.MakeReadOnly());
    }

    public int Count<T> ()
    {
      return CurrentData == null ? 0 : CurrentData.GetResult<T>().Count;
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
