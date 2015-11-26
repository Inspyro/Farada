using System;
using Farada.TestDataGeneration.BaseDomain.Constraints;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Modifiers;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  internal class DomainConfigurator : ChainConfigurator, ITestDataConfigurator
  {
    private bool _useDefaults;
    private Random _random;
    private CompoundValueProviderBuilder _valueProviderBuilder;
    private Func<string, string> _paremeterToPropertyConversionFunc;

    protected internal DomainConfigurator ()
        : base (null)
    {
      _lazyValueProviderBuilder = () => ValueProviderBuilder;

      _random = new Random();
      _useDefaults = true;

      _paremeterToPropertyConversionFunc = parameterName => parameterName[0].ToString().ToUpper() + parameterName.Substring (1);
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

    public ITestDataConfigurator UseParameterToPropertyConversion (Func<string, string> paremeterToPropertyConversionFunc)
    {
      _paremeterToPropertyConversionFunc = paremeterToPropertyConversionFunc;
      return this;
    }

    public ITestDataConfigurator AddInstanceModifier (IInstanceModifier instanceModifier)
    {
      _lazyValueProviderBuilder().AddInstanceModifier (instanceModifier);
      return this;
    }

    private CompoundValueProviderBuilder ValueProviderBuilder
    {
      get { return _valueProviderBuilder ?? (_valueProviderBuilder = CreateValueProviderBuilder()); }
    }

    private CompoundValueProviderBuilder CreateValueProviderBuilder ()
    {
      var valueProviderBuilder = new CompoundValueProviderBuilder (
          _random, new FuncParameterConversionSerivce (_paremeterToPropertyConversionFunc));

      if (!_useDefaults)
        return valueProviderBuilder;

      //Value types:
      // ReSharper disable RedundantTypeArgumentsOfMethod
      valueProviderBuilder.AddProvider<bool, ValueProviderContext<bool>> (new RandomBoolGenerator());
      valueProviderBuilder.AddProvider<byte, ValueProviderContext<byte>> (new RandomByteGenerator());
      valueProviderBuilder.AddProvider<char, ValueProviderContext<char>> (new RandomCharGenerator());
      valueProviderBuilder.AddProvider<decimal, ValueProviderContext<decimal>> (new RandomDecimalGenerator());
      valueProviderBuilder.AddProvider<double, RangeConstrainedValueProviderContext<double>> (new RandomDoubleGenerator());
      valueProviderBuilder.AddProvider<Enum, ValueProviderContext<Enum>> (new RandomEnumGenerator());
      valueProviderBuilder.AddProvider<float, RangeConstrainedValueProviderContext<float>> (new RandomFloatGenerator());
      valueProviderBuilder.AddProvider<int, RangeConstrainedValueProviderContext<int>> (new RandomIntGenerator());
      valueProviderBuilder.AddProvider<long, RangeConstrainedValueProviderContext<long>> (new RandomLongGenerator());
      valueProviderBuilder.AddProvider<sbyte, ValueProviderContext<sbyte>> (new RandomSByteGenerator());
      valueProviderBuilder.AddProvider<short, ValueProviderContext<short>> (new RandomShortGenerator());
      valueProviderBuilder.AddProvider<uint, RangeConstrainedValueProviderContext<uint>> (new RandomUIntGenerator());
      valueProviderBuilder.AddProvider<ulong, RangeConstrainedValueProviderContext<ulong>> (new RandomULongGenerator());
      valueProviderBuilder.AddProvider<ushort, ValueProviderContext<ushort>> (new RandomUShortGenerator());
      valueProviderBuilder.AddProvider<Guid, ValueProviderContext<Guid>> (new RandomGuidGenerator());

      valueProviderBuilder.AddProvider<string, StringConstrainedValueProviderContext> (new RandomWordGenerator (new RandomSyllabileGenerator()));
          //string
      valueProviderBuilder.AddProvider<DateTime, ValueProviderContext<DateTime>> (new RandomDateTimeGenerator());
      // ReSharper restore RedundantTypeArgumentsOfMethod

      //constraint providers
      valueProviderBuilder.AddProvider (new EmailGenerator());

      //default instance provider (also supports subtypes thus object means = all types)
      valueProviderBuilder.AddProvider(new DefaultInstanceValueProvider<object>());

      return valueProviderBuilder;
    }

    private class FuncParameterConversionSerivce : IParameterConversionService
    {
      private readonly Func<string, string> _paremeterToPropertyConversionFunc;

      public FuncParameterConversionSerivce (Func<string, string> paremeterToPropertyConversionFunc)
      {
        _paremeterToPropertyConversionFunc = paremeterToPropertyConversionFunc;
      }

      public string ToPropertyName (string parameterName)
      {
        return _paremeterToPropertyConversionFunc (parameterName);
      }
    }
  }
}