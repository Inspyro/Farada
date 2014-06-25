using System;
using Farada.Evolution.RuleBasedDataGeneration;

namespace Farada.Evolution.IntegrationTests.TestDomain
{
  internal class WorldRule : GlobalRule
  {
    protected override void Execute ()
    {
      World.Write (x => x.Fertility = 100);
    }
  }
}