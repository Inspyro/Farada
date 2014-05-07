using System;
using System.Collections.Generic;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public abstract class Rule<T>:IRule
  {
    public abstract IEnumerable<IRuleParameter> GetRuleInputs ();
    public abstract void Execute (List<IRuleInput> inputData, GeneratorDataProvider generatorDataProvider, CompoundValueProvider valueProvider);

    public Type MainDataType
    {
      get { return typeof (T); }
    }
  }

  public interface IRule
  {
    IEnumerable<IRuleParameter> GetRuleInputs ();
    void Execute (List<IRuleInput> inputData, GeneratorDataProvider dataProvider, CompoundValueProvider valueProvider);
    Type MainDataType { get; }
  }
}