using System;
using Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  public class InitialDataProvider
  {
    private readonly GeneratorDataProvider _generatorDataProvider;

    public InitialDataProvider (GeneratorDataProvider generatorDataProvider)
    {
      _generatorDataProvider = generatorDataProvider;
    }

    public void Add<T>(T value)
    {
      _generatorDataProvider.Add(value);
    }

    public GeneratorResult Build()
    {
      return new GeneratorResult(_generatorDataProvider.DataLists);
    }
  }
}