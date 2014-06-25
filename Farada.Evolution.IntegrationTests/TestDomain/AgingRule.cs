using System;
using System.Collections.Generic;
using Farada.Evolution.RuleBasedDataGeneration;
using Farada.Evolution.Utilities;

namespace Farada.Evolution.IntegrationTests.TestDomain
{
  //TODO: Rule for every generation - not per type
    //TODO: World class that contains global generation data - like World.IsFertile..

  internal class AgingRule : UnrestrictedRule<Person>
  {
    protected override IEnumerable<Person> Execute (RuleValue<Person> person)
    {
      person.UserData.IsPregnant = false;
      person.Value.Age++;

      yield break;
    }
  }
}