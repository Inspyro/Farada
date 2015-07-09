using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public class CompoundRuleInput : IEnumerable<IRuleValue>
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

    public void AddParameterValue(IRuleValue value)
    {
      _values.Add(value);
    }

    public int Count
    {
      get { return _values.Count; }
    }

    IEnumerator<IRuleValue> IEnumerable<IRuleValue>.GetEnumerator ()
    {
      return _values.GetEnumerator();
    }

    public IEnumerator GetEnumerator ()
    {
      return _values.GetEnumerator();
    }

    public IRuleValue this [int index]
    {
      get { return _values[index]; }
      set { _values[index] = value; }
    }

    public bool IsValidFor (IEnumerable<IRuleParameter> inputParameters)
    {
      return
          _values.All (
              value =>
                  inputParameters.Where (inputParameter => inputParameter.DataType == value.GetDataType())
                      .All (inputParameter => inputParameter.ParameterPredicate (value, this) == null));
    }
  }
  
}