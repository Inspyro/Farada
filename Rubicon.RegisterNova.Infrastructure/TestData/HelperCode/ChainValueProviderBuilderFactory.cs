using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode
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
      defaultProvider.SetProvider(new BasicStringGenerator());

      return defaultProvider;
    }

    public ChainValueProviderBuilder GetEmpty ()
    {
      return new ChainValueProviderBuilder(_randomGeneratorProvider);
    }
  }
}