using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration
{
  public interface IRandomGenerator
  {
    void SetRandom (Random random);
    object NextObject ();
  }

  public abstract class RandomGenerator<T>:IRandomGenerator
  {
    protected Random Random { get; private set; }

    public void SetRandom (Random random)
    {
      Random = random;
    }

    public object NextObject ()
    {
      return Next();
    }

    public abstract T Next ();
  }
}