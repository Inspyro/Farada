using System;

namespace Farada.Evolution.RuleBasedDataGenerator
{
  public abstract class GlobalRule:IGlobalRule
  {
    protected IWriteableWorld World { get; private set; }
    protected abstract void Execute ();

    public void Execute (IWriteableWorld world)
    {
      World = world;
      Execute();
    }
  }

  public interface IGlobalRule
  {
    void Execute (IWriteableWorld world);
  }
}