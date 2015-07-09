using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.Extensions;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public class GeneratorDataProvider
  {
    private readonly Dictionary<Type, LinkedList<IRuleValue>> _dataLists;

    internal GeneratorDataProvider(Dictionary<Type, LinkedList<IRuleValue>> initialData=null)
    {
      _dataLists = initialData ?? new Dictionary<Type, LinkedList<IRuleValue>>();
    }

    internal Dictionary<Type, LinkedList<IRuleValue>> DataLists { get { return _dataLists; } }

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

      LinkedList<IRuleValue> valueList;
      if(_dataLists.ContainsKey(dataType))
      {
        valueList = _dataLists[dataType];
      }
      else
      {
        valueList = new LinkedList<IRuleValue>();
        _dataLists.Add(dataType, valueList);
      }

      var valueNode = new LinkedListNode<IRuleValue> (value);

      if (valueList.Last == null)
        valueList.AddLast (valueNode);
      else
        valueList.AddAfter (valueList.Last, valueNode);

      value.OnDeleted(() => Delete(dataType, valueNode));
    }

    private void Delete (Type dataType, LinkedListNode<IRuleValue> nodeToDelete)
    {
      if (!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      var valueList = _dataLists[dataType];
      valueList.Remove (nodeToDelete);
    }
  }
}