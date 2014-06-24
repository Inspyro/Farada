using System;
using System.Collections.Generic;
using System.Linq;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public class GeneratorResult
  {
    private readonly Dictionary<Type,  List<IRuleValue>> _dataLists;
    internal GeneratorResult(Dictionary<Type, List<IRuleValue>> dataLists)
    {
      _dataLists = dataLists;
    }

    internal Dictionary<Type,  List<IRuleValue>> DataLists { get { return _dataLists; } }

    public IList<T> GetResult<T>()
    {
      var dataType = typeof (T);
      if(!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      return _dataLists[dataType].Select(value => value.GetValue<T>()).ToList();
    }

    public IList<Type> GetResultTypes()
    {
      return _dataLists.Keys.ToList();
    }
  }
}