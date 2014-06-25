using System;
using System.Collections.Generic;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public abstract class Rule:IRule
  {
    public abstract IEnumerable<IRuleParameter> GetRuleInputs (IReadableWorld world);
    public abstract IEnumerable<IRuleValue> Execute (CompoundRuleExecutionContext context);

    public virtual float GetExecutionProbability (IReadableWorld world)
    {
      return 1;
    }
  }

  public interface IRule
  {
    IEnumerable<IRuleParameter> GetRuleInputs (IReadableWorld world);
    IEnumerable<IRuleValue> Execute (CompoundRuleExecutionContext context);
    float GetExecutionProbability (IReadableWorld world);
  }
}