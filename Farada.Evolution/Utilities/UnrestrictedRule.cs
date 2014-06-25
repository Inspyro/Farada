using System;
using System.Collections.Generic;
using System.Linq;
using Farada.Evolution.RuleBasedDataGeneration;

namespace Farada.Evolution.Utilities
{
  public abstract class UnrestrictedRule<T>:Rule
  {
    public sealed override float GetExecutionProbability (IReadableWorld world)
    {
      return 1f;
    }

    public sealed override IEnumerable<IRuleParameter> GetRuleInputs (IReadableWorld world)
    {
      yield return new RuleParameter<T>();
    }

    public sealed override IEnumerable<IRuleValue> Execute (CompoundRuleExecutionContext context)
    {
      return Execute(new SimpleRuleExecutionContext<T>(context.TestDataGenerator, context.World, context.InputData.GetValue<T>(0))).Select(result => new RuleValue<T>(result));
    }

    protected abstract IEnumerable<T> Execute (SimpleRuleExecutionContext<T> context);
  }
}
