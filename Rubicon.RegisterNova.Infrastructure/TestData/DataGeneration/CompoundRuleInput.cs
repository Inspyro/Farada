using System;
using System.Collections.Generic;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public class CompoundRuleInput
  {
    private readonly List<IRuleValue> _values;
    public CompoundRuleInput()
    {
      _values = new List<IRuleValue>();
    }

    public RuleValue<T1> GetValue<T1> (int index)
    {
      return (RuleValue<T1>) _values[index];
    }

    public void Add(IRuleValue value)
    {
      _values.Add(value);
    }

    public int Count
    {
      get { return _values.Count; }
    }
  }

  
}