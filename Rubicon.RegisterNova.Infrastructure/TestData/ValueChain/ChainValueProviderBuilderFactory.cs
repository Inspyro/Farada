using System;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class ChainValueProviderBuilderFactory
  {
    private readonly RandomGeneratorProvider _randomGeneratorProvider;

    internal ChainValueProviderBuilderFactory(RandomGeneratorProvider randomGeneratorProvider)
    {
      _randomGeneratorProvider = randomGeneratorProvider;
    }

    public ChainValueProviderBuilder GetDefault ()
    {
      var defaultProvider = GetEmpty();
      defaultProvider.SetProvider(new FuncProvider<string>((randomGenerator) => randomGenerator.Next()));

      return defaultProvider;
    }

    public ChainValueProviderBuilder GetEmpty ()
    {
      return new ChainValueProviderBuilder(_randomGeneratorProvider);
    }
  }
}