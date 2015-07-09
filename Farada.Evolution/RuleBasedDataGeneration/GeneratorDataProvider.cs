using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.Extensions;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public class GeneratorDataProvider
  {
    private readonly Random _random;
    private readonly Dictionary<Type, List<IRuleValue>> _dataLists; 
    internal GeneratorDataProvider(Random random, Dictionary<Type, List<IRuleValue>> initialData=null)
    {
      _random = random;
      _dataLists = initialData ?? new Dictionary<Type, List<IRuleValue>>();
    }

    internal Dictionary<Type, List<IRuleValue>> DataLists { get { return _dataLists; } }

    internal IEnumerable<IRuleValue> GetAll (IRuleParameter ruleParameter)
    {
      var dataType = ruleParameter.DataType;
      if (!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      return _dataLists[dataType].WhereValues(ruleParameter.Predicate);
    }

    public void Add(IRuleValue value)
    {
      var dataType = value.GetDataType();

      List<IRuleValue> valueList;
      if(_dataLists.ContainsKey(dataType))
      {
        valueList = _dataLists[dataType];
      }
      else
      {
        valueList = new List<IRuleValue>();
        _dataLists.Add(dataType, valueList);
      }

      value.OnDeleted(() => Delete(dataType, value));
      valueList.Add(value);
    }

    private void Delete (Type dataType, IRuleValue value)
    {
      if (!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      var valueList = _dataLists[dataType];
      valueList.Remove(value);
    }
  }
}