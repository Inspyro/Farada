using System;
using System.ComponentModel.DataAnnotations;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;

namespace Farada.TestDataGeneration.CompoundValueProviders
{
  /// <summary>
  /// Builds a <see cref="CompoundValueProviderBuilder"/> based on the given <see cref="Random"/>
  /// </summary>
  internal class CompoundValueProviderBuilderFactory
  {
    private readonly Random _random;

    internal CompoundValueProviderBuilderFactory(Random random)
    {
      _random = random;
    }

    /// <summary>
    /// Creates an empty value provider and injects all value providers of the base domain <see cref="Farada.TestDataGeneration.BaseDomain.ValueProviders"/>
    /// </summary>
    /// <returns>the compound value provider builder containing the base domain value providers</returns>
    internal CompoundValueProviderBuilder GetDefault ()
    {
      var defaultProvider = GetEmpty();

      //Value types:
      defaultProvider.AddProvider((bool b) => b, new RandomBoolGenerator());
      defaultProvider.AddProvider((byte b) => b, new RandomByteGenerator());
      defaultProvider.AddProvider((char c) => c, new RandomCharGenerator());
      defaultProvider.AddProvider((decimal d) => d, new RandomDecimalGenerator());
      defaultProvider.AddProvider((double d) => d, new RandomDoubleGenerator());
      defaultProvider.AddProvider((Enum e) => e, new RandomEnumGenerator());
      defaultProvider.AddProvider((float f) => f, new RandomFloatGenerator());
      defaultProvider.AddProvider((int i) => i, new RandomIntGenerator());
      defaultProvider.AddProvider((long l) => l, new RandomLongGenerator());
      defaultProvider.AddProvider((sbyte sb) => sb, new RandomSByteGenerator());
      defaultProvider.AddProvider((short s) => s, new RandomShortGenerator());
      defaultProvider.AddProvider((uint ui) => ui, new RandomUIntGenerator());
      defaultProvider.AddProvider((ulong ul) => ul, new RandomULongGenerator());
      defaultProvider.AddProvider((ushort us) => us, new RandomUShortGenerator());

      defaultProvider.AddProvider((string s)=>s, new RandomWordGenerator(new RandomSyllabileGenerator())); //string
      defaultProvider.AddProvider((DateTime dt)=>dt, new RandomPastDateTimeGenerator());

      //constraint providers
      defaultProvider.AddProvider(new EmailGenerator());

      return defaultProvider;
    }

    /// <summary>
    /// creates an empty compound value provider, that does not declare any way to fill a value
    /// </summary>
    /// <returns>an empty compound value provider</returns>
    internal CompoundValueProviderBuilder GetEmpty ()
    {
      return new CompoundValueProviderBuilder(_random);
    }
  }
}