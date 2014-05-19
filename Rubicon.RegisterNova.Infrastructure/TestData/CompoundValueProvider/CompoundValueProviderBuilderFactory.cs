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

      //Value types:
      defaultProvider.SetProvider(new RandomBoolGenerator());
      defaultProvider.SetProvider(new RandomByteGenerator());
      defaultProvider.SetProvider(new RandomCharGenerator());
      defaultProvider.SetProvider(new RandomDecimalGenerator());
      defaultProvider.SetProvider(new RandomDoubleGenerator());
      defaultProvider.SetProvider(new RandomEnumGenerator());
      defaultProvider.SetProvider(new RandomFloatGenerator());
      defaultProvider.SetProvider(new RandomIntGenerator());
      defaultProvider.SetProvider(new RandomLongGenerator());
      defaultProvider.SetProvider(new RandomSByteGenerator());
      defaultProvider.SetProvider(new RandomShortGenerator());
      defaultProvider.SetProvider(new RandomUIntGenerator());
      defaultProvider.SetProvider(new RandomULongGenerator());
      defaultProvider.SetProvider(new RandomUShortGenerator());


      defaultProvider.SetProvider(new RandomWordGenerator()); //string
      defaultProvider.SetProvider(new RandomDateTimeGenerator());

      return defaultProvider;
    }

    internal CompoundValueProviderBuilder GetEmpty ()
    {
      return new CompoundValueProviderBuilder(_random);
    }
  }
}