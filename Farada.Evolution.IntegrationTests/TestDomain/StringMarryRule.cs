using System;
using System.Collections.Generic;
using Farada.Evolution.RuleBasedDataGeneration;

namespace Farada.Evolution.IntegrationTests.TestDomain
{
  class StringMarryRule : Rule
  {
    public override float GetExecutionProbability ()
    {
      return 0.5f;
    }

    protected override IEnumerable<IRuleParameter> GetRuleInputs ()
    {
      Func<RuleValue<string>, bool> predicate = p => p.Value.Length > 3 && (p.UserData.IsMarried == null || !p.UserData.IsMarried);
      yield return new RuleParameter<string> (predicate);
      yield return new RuleParameter<string> (predicate); //TODO: how to define excludes on rule filter basis?
    }

    protected override IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData) //e.g. one instance - stores all generation data..
    {
      var sexyString1 = inputData.GetValue<string> (0);
      var sexyString2 = inputData.GetValue<string> (1);

      sexyString1.Value += "[Married to:" + sexyString2.Value + "]";
      sexyString2.Value += "[Married to:" + sexyString1.Value + "]";

      sexyString1.UserData.IsMarried = true;
      sexyString2.UserData.IsMarried = true;

      yield break;
    }
  }
}