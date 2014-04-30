using System;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode;
using Rubicon.RegisterNova.Infrastructure.TestData.SampleCode;
using Rubicon.RegisterNova.Infrastructure.TestData.SampleCode.ValueProviders;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  internal static class ChainValueProviderBuilderFactory
  {
    public static ChainValueProviderBuilder GetDefault ()
    {
      var defaultProvider = GetEmpty();
      defaultProvider.SetProvider(new BasicStringGenerator(new CustomStringGenerator()));

      return defaultProvider;
    }

    public static ChainValueProviderBuilder GetEmpty ()
    {
      return new ChainValueProviderBuilder();
    }
  }
}