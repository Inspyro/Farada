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
      defaultProvider.AddProvider(new RandomBoolGenerator());
      defaultProvider.AddProvider(new RandomByteGenerator());
      defaultProvider.AddProvider(new RandomCharGenerator());
      defaultProvider.AddProvider(new RandomDecimalGenerator());
      defaultProvider.AddProvider(new RandomDoubleGenerator());
      defaultProvider.AddProvider(new RandomEnumGenerator());
      defaultProvider.AddProvider(new RandomFloatGenerator());
      defaultProvider.AddProvider(new RandomIntGenerator());
      defaultProvider.AddProvider(new RandomLongGenerator());
      defaultProvider.AddProvider(new RandomSByteGenerator());
      defaultProvider.AddProvider(new RandomShortGenerator());
      defaultProvider.AddProvider(new RandomUIntGenerator());
      defaultProvider.AddProvider(new RandomULongGenerator());
      defaultProvider.AddProvider(new RandomUShortGenerator());


      defaultProvider.AddProvider(new RandomWordGenerator()); //string
      defaultProvider.AddProvider(new RandomPastDateTimeGenerator());

      return defaultProvider;
    }

    internal CompoundValueProviderBuilder GetEmpty ()
    {
      return new CompoundValueProviderBuilder(_random);
    }
  }
}