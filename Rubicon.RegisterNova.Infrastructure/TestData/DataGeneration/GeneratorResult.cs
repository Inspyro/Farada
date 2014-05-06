using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public class GeneratorResult
  {
    private readonly Dictionary<Type, IList> _dataLists;
    internal GeneratorResult(Dictionary<Type, IList> dataLists)
    {
      _dataLists = dataLists;
    }

    internal Dictionary<Type, IList> DataLists { get { return _dataLists; } }

    public IList<T> GetResult<T>()
    {
      var dataType = typeof (T);
      if(!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      return ((IList<Handle<T>>) _dataLists[dataType]).Select(handle => handle.Value).ToList();
    }

    public IList<Type> GetResultTypes()
    {
      return _dataLists.Keys.ToList();
    }
  }
}