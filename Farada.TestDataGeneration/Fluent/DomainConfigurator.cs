using System;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Modifiers;

namespace Farada.TestDataGeneration.Fluent
{
  internal class DomainConfigurator: ChainConfigurator, ITestDataConfigurator

  {
    private bool _useDefaults;
    private Random _random;

    protected internal DomainConfigurator ()
        : base (null)
    {
      _lazyValueProviderBuilder = () => ValueProviderBuilder;
    }

    public ITestDataConfigurator UseDefaults (bool useDefaults)
    {
      _useDefaults = useDefaults;
      return this;
    }

    public ITestDataConfigurator UseRandom (Random random)
    {
      _random = random;
      return this;
    }

    public ITestDataConfigurator AddInstanceModifier (IInstanceModifier instanceModifier)
    {
      _lazyValueProviderBuilder().AddInstanceModifier(instanceModifier);
      return this;
    }

    private CompoundValueProviderBuilder _valueProviderBuilder;
    private CompoundValueProviderBuilder ValueProviderBuilder
    {
      get { return _valueProviderBuilder ?? (_valueProviderBuilder = CreateValueProviderBuilder()); }
    }

    private CompoundValueProviderBuilder CreateValueProviderBuilder()
    {
      var valueProviderBuilder = new CompoundValueProviderBuilder (_random);

      if (!_useDefaults)
        return valueProviderBuilder;

       //Value types:
      valueProviderBuilder.AddProvider((bool b) => b, new RandomBoolGenerator());
      valueProviderBuilder.AddProvider((byte b) => b, new RandomByteGenerator());
      valueProviderBuilder.AddProvider((char c) => c, new RandomCharGenerator());
      valueProviderBuilder.AddProvider((decimal d) => d, new RandomDecimalGenerator());
      valueProviderBuilder.AddProvider((double d) => d, new RandomDoubleGenerator());
      valueProviderBuilder.AddProvider((Enum e) => e, new RandomEnumGenerator());
      valueProviderBuilder.AddProvider((float f) => f, new RandomFloatGenerator());
      valueProviderBuilder.AddProvider((int i) => i, new RandomIntGenerator());
      valueProviderBuilder.AddProvider((long l) => l, new RandomLongGenerator());
      valueProviderBuilder.AddProvider((sbyte sb) => sb, new RandomSByteGenerator());
      valueProviderBuilder.AddProvider((short s) => s, new RandomShortGenerator());
      valueProviderBuilder.AddProvider((uint ui) => ui, new RandomUIntGenerator());
      valueProviderBuilder.AddProvider((ulong ul) => ul, new RandomULongGenerator());
      valueProviderBuilder.AddProvider((ushort us) => us, new RandomUShortGenerator());

      valueProviderBuilder.AddProvider((string s)=>s, new RandomWordGenerator(new RandomSyllabileGenerator())); //string
      valueProviderBuilder.AddProvider((DateTime dt)=>dt, new RandomDateTimeGenerator());

      //constraint providers
      valueProviderBuilder.AddProvider(new EmailGenerator());

      return valueProviderBuilder;
    }
  }
}