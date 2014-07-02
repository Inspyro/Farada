using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.CompoundValueProvider;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public abstract class Rule:IRule
  {
    protected ICompoundValueProvider ValueProvider { get; private set; }
    protected IReadableWorld World { get; private set; }

    protected abstract IEnumerable<IRuleParameter> GetRuleInputs ();
    protected abstract IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData);

    public IEnumerable<IRuleParameter> GetRuleInputs (IReadableWorld world)
    {
      World = world;
      return GetRuleInputs();
    }

    public IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData, ICompoundValueProvider valueProvider, IReadableWorld world)
    {
      ValueProvider = valueProvider;
      return Execute(inputData);
    }

    public virtual float GetExecutionProbability ()
    {
      return 1;
    }
  }

  public interface IRule
  {
    IEnumerable<IRuleParameter> GetRuleInputs (IReadableWorld world);
    IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData, ICompoundValueProvider valueProvider, IReadableWorld world);
    float GetExecutionProbability ();
  }
}