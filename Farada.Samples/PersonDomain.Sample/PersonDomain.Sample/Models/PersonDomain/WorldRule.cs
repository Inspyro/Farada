using System;
using Farada.Evolution.RuleBasedDataGeneration;
using Farada.Evolution.Utilities;

namespace PersonDomain.Sample.Models.PersonDomain
{
  internal class WorldRule : GlobalRule
  {
    protected override void Execute ()
    {
      World.Write (x => x.Fertility = LerpUtility.LerpFromLowToHigh (100000, World.Count<Person> (), 1f, 0.1f));
    }
  }
}