using System;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public interface IGlobalRule
  {
    void Execute (IWriteableWorld world);
  }
}