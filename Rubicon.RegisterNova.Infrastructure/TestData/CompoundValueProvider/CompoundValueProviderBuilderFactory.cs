using System;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.CompoundValueProvider;
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

      defaultProvider.AddProvider<bool?>(ctx => ctx.ValueProvider.Create<bool>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<byte?>(ctx => ctx.ValueProvider.Create<byte>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<char?>(ctx => ctx.ValueProvider.Create<char>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<decimal?>(ctx => ctx.ValueProvider.Create<decimal>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<double?>(ctx => ctx.ValueProvider.Create<double>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<float?>(ctx => ctx.ValueProvider.Create<float>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<int?>(ctx => ctx.ValueProvider.Create<int>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<long?>(ctx => ctx.ValueProvider.Create<long>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<sbyte?>(ctx => ctx.ValueProvider.Create<sbyte>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<short?>(ctx => ctx.ValueProvider.Create<short>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<uint?>(ctx => ctx.ValueProvider.Create<uint>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<ulong?>(ctx => ctx.ValueProvider.Create<ulong>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider<ushort?>(ctx => ctx.ValueProvider.Create<ushort>(propertyInfo: ctx.PropertyInfo));

      defaultProvider.AddProvider(new RandomWordGenerator()); //string
      defaultProvider.AddProvider(new RandomPastDateTimeGenerator());

      defaultProvider.AddProvider<DateTime?>(ctx => ctx.ValueProvider.Create<DateTime>(propertyInfo: ctx.PropertyInfo)); //TODO: Create<DateTime> calls event date time generator, but then r

      //constraint providers
      defaultProvider.AddProvider(new EmailGenerator());

      return defaultProvider;
    }

    internal CompoundValueProviderBuilder GetEmpty ()
    {
      return new CompoundValueProviderBuilder(_random);
    }
  }
}