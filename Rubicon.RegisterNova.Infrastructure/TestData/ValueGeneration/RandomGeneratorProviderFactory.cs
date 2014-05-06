using System;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.String;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration
{
  public class RandomGeneratorProviderFactory
  {
    private readonly Random _random;

    public RandomGeneratorProviderFactory(Random random)
    {
      _random = random;
    }

    public RandomGeneratorProvider GetDefault()
    {
      var defaultProvider = GetEmpty();
      defaultProvider.SetBase(new RandomStringGenerator());

      return defaultProvider;
    }

    public RandomGeneratorProvider GetEmpty()
    {
       return new RandomGeneratorProvider(_random);
    }
  }
}
