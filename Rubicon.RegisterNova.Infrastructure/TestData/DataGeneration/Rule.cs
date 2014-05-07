using System;
using System.Collections.Generic;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public abstract class Rule:IRule
  {
    protected CompoundValueProvider ValueProvider { get; private set; }
    protected IReadableWorld World { get; private set; }

    protected abstract IEnumerable<IRuleParameter> GetRuleInputs ();
    protected abstract IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData);

    public IEnumerable<IRuleParameter> GetRuleInputs (IReadableWorld world)
    {
      World = world;
      return GetRuleInputs();
    }

    public IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData, CompoundValueProvider valueProvider, IReadableWorld world)
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
    IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData, CompoundValueProvider valueProvider, IReadableWorld world);
    float GetExecutionProbability ();
  }
}