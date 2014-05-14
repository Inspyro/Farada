using System;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.RuleBasedDataGenerator;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.RuleBasedDataGeneration
{
  public abstract class UnrestrictedRule<T>:Rule
  {
    public sealed override float GetExecutionProbability ()
    {
      return 1f;
    }

    protected sealed override IEnumerable<IRuleParameter> GetRuleInputs ()
    {
      yield return new RuleParameter<T>();
    }

    protected sealed override IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData)
    {
      return Execute(inputData.GetValue<T>(0)).Select(result => new RuleValue<T>(result));
    }

    protected abstract IEnumerable<T> Execute (RuleValue<T> inputData);
  }
}
