using System;
using System.Collections.Generic;
using Farada.Evolution.RuleBasedDataGeneration;
using Farada.Evolution.Utilities;

namespace PersonDomain.Sample.Models.PersonDomain
{
  internal class AgingRule : UnrestrictedRule<Person>
  {
    protected override IEnumerable<Person> Execute (SimpleRuleExecutionContext<Person> context)
    {
      context.Input.UserData.IsPregnant = false;
      context.Input.Value.Age++;

      yield break;
    }
  }
}