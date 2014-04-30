using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode
{
  public static class ChainValueProviderBuilderFactory
  {
    public static ChainValueProviderBuilder GetDefault ()
    {
      var defaultProvider = GetEmpty();
      defaultProvider.SetProvider(new BasicStringGenerator());

      return defaultProvider;
    }

    public static ChainValueProviderBuilder GetEmpty ()
    {
      return new ChainValueProviderBuilder();
    }
  }
}