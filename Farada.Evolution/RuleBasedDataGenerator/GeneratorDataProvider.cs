using System;
using System.Collections.Generic;
using Farada.Core.Extensions;

namespace Farada.Evolution.RuleBasedDataGenerator
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

      return _dataLists[dataType].WhereValues(ruleParameter.Predicate, ruleParameter.Excludes);
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

      var indexInList = valueList.Count;

      value.OnDeleted(() => Delete(dataType, indexInList));
      valueList.Add(value);
    }

    private void Delete (Type dataType, int index)
    {
      if (!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      var valueList = _dataLists[dataType];
      valueList.RemoveAt(index);
    }
  }
}