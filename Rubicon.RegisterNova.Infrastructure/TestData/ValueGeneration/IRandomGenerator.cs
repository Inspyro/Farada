using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration
{
  public interface IRandomGenerator
  {
    object NextObject ();
  }

  public abstract class RandomGenerator<T>:IRandomGenerator
  {
    public object NextObject ()
    {
      return Next();
    }

    public abstract T Next ();
    public Random Random { get; set; }
  }
}