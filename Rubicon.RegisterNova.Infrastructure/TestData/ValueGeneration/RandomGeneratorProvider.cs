using System;
using System.Collections.Generic;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration
{
  public class RandomGeneratorProvider
  {
    public Random Random { get; private set; }

    private readonly Dictionary<Type, IRandomGenerator> _typeToBaseGenerator;
    private readonly Dictionary<Type, Dictionary<Type, IRandomGenerator>> _typeToConcreteGenerator;
 
    public RandomGeneratorProvider(Random random)
    {
      Random = random;
      _typeToBaseGenerator = new Dictionary<Type, IRandomGenerator>();
      _typeToConcreteGenerator = new Dictionary<Type, Dictionary<Type, IRandomGenerator>>();
    }

    public RandomGenerator<T> Get<T> ()
    {
      var returnType = typeof (T);

      if (!_typeToBaseGenerator.ContainsKey(returnType))
      {
        return null;
      }

      return (RandomGenerator<T>) _typeToBaseGenerator[returnType];
    }

    public RandomGenerator<T> Get<T> (Type generatorType)
    {
       var returnType = typeof (T);

      if (!_typeToConcreteGenerator.ContainsKey(returnType))
      {
        return null;
      }

      var concreteGenerators = _typeToConcreteGenerator[returnType];
      if(!concreteGenerators.ContainsKey(generatorType))
      {
        return null;
      }

      return (RandomGenerator<T>) concreteGenerators[generatorType];

    }

    public void Add<T>(RandomGenerator<T> randomGenerator)
    {
      randomGenerator.Random = Random;

      var returnType = typeof (T);
      Dictionary<Type, IRandomGenerator> concreteGenerators;

      if(_typeToConcreteGenerator.ContainsKey(returnType))
      {
        concreteGenerators =_typeToConcreteGenerator[returnType];
      }
      else
      {
        concreteGenerators = new Dictionary<Type, IRandomGenerator>();
        _typeToConcreteGenerator.Add(returnType, concreteGenerators);
      }

      var generatorType = randomGenerator.GetType();
      if(concreteGenerators.ContainsKey(generatorType))
      {
        concreteGenerators[generatorType] = randomGenerator;
      }
      else
      {
        concreteGenerators.Add(generatorType, randomGenerator);
      }
    }

    public void SetBase<T>(RandomGenerator<T> baseGenerator)
    {
      baseGenerator.Random = Random;

      var returnType = typeof (T);
      if(_typeToBaseGenerator.ContainsKey(returnType))
      {
        _typeToBaseGenerator[returnType] = baseGenerator;
      }
      else
      {
        _typeToBaseGenerator.Add(returnType, baseGenerator);
      }
    }
  }
}