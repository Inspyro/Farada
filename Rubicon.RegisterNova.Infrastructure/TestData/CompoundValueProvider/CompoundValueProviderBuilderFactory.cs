using System;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  internal class CompoundValueProviderBuilderFactory
  {
    private readonly Random _random;

    internal CompoundValueProviderBuilderFactory(Random random)
    {
      _random = random;
    }

    internal CompoundValueProviderBuilder GetDefault ()
    {
      var defaultProvider = GetEmpty();
      defaultProvider.SetProvider(new RandomStringGenerator());
      //TODO: Add default providers, e.g. int, DateTime, IEnumerable!

      return defaultProvider;
    }

    internal CompoundValueProviderBuilder GetEmpty ()
    {
      return new CompoundValueProviderBuilder(_random);
    }
  }
}