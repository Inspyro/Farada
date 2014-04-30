using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  public class TestDataGeneratorFactory
  {
    public ChainValueProviderBuilderFactory ValueProviderBuilderFactory { get; private set; }

    public TestDataGeneratorFactory(RandomGeneratorProvider randomGeneratorProvider)
    {
      ValueProviderBuilderFactory = new ChainValueProviderBuilderFactory(randomGeneratorProvider);
    }

    public TestDataGenerator Build (ChainValueProviderBuilder valueProviderBuilder)
    {
      return new TestDataGenerator(new TypeValueProvider(valueProviderBuilder.ToValueProvider()));
    }
  }

  public class TestDataGenerator
  {
    public TypeValueProvider ValueProvider { get; private set; }

    internal TestDataGenerator(TypeValueProvider valueProvider)
    {
      ValueProvider = valueProvider;
    }

    public GeneratorResult Generate(Domain domain)
    {
      //here we need to calculate multiple generations - how to specify the end/abort condition - maybe fixed generation count, or target data count, 
      //or manually calling generate multiple times?
      return new GeneratorResult(new Dictionary<Type, IList>()); //here we need to get the result from the data generator / data holder etc...
    }
  }

  public class GeneratorResult
  {
    private readonly Dictionary<Type, IList> _dataLists;
    internal GeneratorResult(Dictionary<Type, IList> dataLists)
    {
      _dataLists = dataLists;
    }

    public IList<T> GetResult<T>()
    {
      var dataType = typeof (T);
      if(!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      return (IList<T>) _dataLists[dataType];
    }

    public IList<Type> GetResultTypes()
    {
      return _dataLists.Keys.ToList();
    }
  }

  class Rule
  {
    void Execute(GeneratorDataProvider dataProvider) //e.g. one instance - stores all generation data..
    {
      var p1 = dataProvider.Get<string>(p => p.Length == 3); //TODO: return null instead of exception if no matching data was found
      var p2 = dataProvider.Get(exclude: p1); 

      if(p1==null||p2==null) //here we abort the rule as we need 2 persons that match our criteria
        return;

      var car = dataProvider.Get<int>();

      var result = p1 == p2;
      dataProvider.Set(result);
    }
  }

  internal class GeneratorDataProvider
  {
    private readonly Dictionary<Type, IList> _dataLists; 
    internal GeneratorDataProvider()
    {
      _dataLists = new Dictionary<Type, IList>();
    }

    public T Get<T> (Func<T, bool> predicate)
    {
      var dataType = typeof (T);
      if(!_dataLists.ContainsKey(dataType))
      {
        throw new ArgumentException("Could not find a result of type " + dataType);
      }

      return ((IList<T>) _dataLists[dataType]).First(predicate); //TODO: Random linq list extensions..
    }

    public T Get<T> ()
    {
      return default(T); //TODO: think of way to use func with predicate e.g. let predicate be null?
    }

    public T Get<T> (T exclude)
    {
      return default(T); //TODO: think of fast way to exclude items from list? maybe just randomly select till it is not selected - but how about 2 entries- maybe use next or previous then ;)?
    }

    public void Set<T> (T result)
    {
      //TODO: Here we need to create new data, add it to the dictionary? Check a way to apply changes to value types / remove value types - maybe through a handle solution -e.g. handle.DeleteValue(), handle.Value, handle.UpdateValue() - necessary?
    }
  }

  public class Domain
  {
     
  }
}
