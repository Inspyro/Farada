using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
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
      _generatorDataProvider.Add(new RuleValue<T>(value));
    }

    public GeneratorResult Build()
    {
      return new GeneratorResult(_generatorDataProvider.DataLists);
    }
  }
}