using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  internal class World: IWriteableWorld
  {
    public int Count<T> ()
    {
      return 0; //TODO:
    }
  }

  public interface IWriteableWorld:IReadableWorld
  {
  }

  public interface IReadableWorld
  {
    int Count<T> ();
  }
}
