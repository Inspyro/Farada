using System;
using System.Collections.Generic;
using Farada.Evolution.RuleBasedDataGeneration;

namespace Farada.Evolution.IntegrationTests.TestDomain
{
  class StringMarryRule : Rule
  {
    public override float GetExecutionProbability (IReadableWorld world)
    {
      return 0.5f;
    }

    public override IEnumerable<IRuleParameter> GetRuleInputs (IReadableWorld world)
    {
      Func<RuleValue<string>, bool> predicate = p => p.Value.Length > 3 && (p.UserData.IsMarried == null || !p.UserData.IsMarried);
      yield return new RuleParameter<string> (predicate);
      yield return new RuleParameter<string> (predicate); //TODO: how to define excludes on rule filter basis?
    }

    public override IEnumerable<IRuleValue> Execute (CompoundRuleExecutionContext context) //e.g. one instance - stores all generation data..
    {
      var sexyString1 = context.InputData.GetValue<string> (0);
      var sexyString2 = context.InputData.GetValue<string> (1);

      sexyString1.Value += "[Married to:" + sexyString2.Value + "]";
      sexyString2.Value += "[Married to:" + sexyString1.Value + "]";

      sexyString1.UserData.IsMarried = true;
      sexyString2.UserData.IsMarried = true;

      yield break;
    }
  }
}