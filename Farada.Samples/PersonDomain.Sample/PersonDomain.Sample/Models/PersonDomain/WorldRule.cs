using System;
using Farada.Evolution.RuleBasedDataGeneration;
using Farada.Evolution.Utilities;

namespace PersonDomain.Sample.Models.PersonDomain
{
  internal class WorldRule : IGlobalRule
  {
    public void Execute (IWriteableWorld world)
    {
      world.Write (x => x.Fertility = LerpUtility.LerpFromLowToHigh (100000, world.Count<Person> (), 1f, 0.1f));
    }
  }
}