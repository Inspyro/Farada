using System;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.String;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class ChainValueProviderBuilderFactory
  {
    private readonly Random _random;

    internal ChainValueProviderBuilderFactory(Random random)
    {
      _random = random;
    }

    internal CompoundValueProviderBuilder GetDefault ()
    {
      var defaultProvider = GetEmpty();
      defaultProvider.SetProvider(new RandomStringGenerator());

      return defaultProvider;
    }

    internal CompoundValueProviderBuilder GetEmpty ()
    {
      return new CompoundValueProviderBuilder(_random);
    }
  }
}