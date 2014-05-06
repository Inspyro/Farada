using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public class GeneratorDataProvider
  {
    private readonly Random _random;
    private readonly Dictionary<Type, IList> _dataLists; 
    internal GeneratorDataProvider(Random random, Dictionary<Type, IList> initialData=null)
    {
      _random = random;
      _dataLists = initialData ?? new Dictionary<Type, IList>();
    }

    internal Dictionary<Type, IList> DataLists { get { return _dataLists; } }

    internal IEnumerable<IRuleInput> GetAll (IRuleParameter ruleParameter)
    {
      var dataType = ruleParameter.DataType;
      if (!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      return _dataLists[dataType].Adapt().WhereValues(ruleParameter.Predicate, ruleParameter.Excludes).Select(ruleParameter.GetRuleInput);
    }

    public void Add<T> (T value)
    {
      var dataType = typeof (T);

      IList<Handle<T>> valueList;
      if(_dataLists.ContainsKey(dataType))
      {
        valueList = (IList<Handle<T>>) _dataLists[dataType];
      }
      else
      {
        valueList = new List<Handle<T>>();
        _dataLists.Add(dataType, (IList) valueList);
      }

      var indexInList = valueList.Count;
      var valueHandle = new Handle<T>(value, () => Delete<T>(indexInList));

      valueList.Add(valueHandle);
    }

    private void Delete<T> (int index)
    {
      var dataType = typeof (T);
      if (!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      var valueList = (IList<Handle<T>>) _dataLists[dataType];
      valueList.RemoveAt(index);
    }

    internal int GetCountInternal (Type dataType)
    {
      if (!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      return _dataLists[dataType].Count;
    }


    internal IList GetHandleListInternal (Type dataType)
    {
    if (!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      return _dataLists[dataType];
    }
  }
}