using System;
using System.ComponentModel.DataAnnotations;
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
      defaultProvider.AddProvider((bool b) => b, new RandomBoolGenerator());
      defaultProvider.AddProvider((byte b) => b, new RandomByteGenerator());
      defaultProvider.AddProvider((char b) => b, new RandomCharGenerator());
      defaultProvider.AddProvider((decimal b) => b, new RandomDecimalGenerator());
      defaultProvider.AddProvider((double b) => b, new RandomDoubleGenerator());
      defaultProvider.AddProvider((Enum b) => b, new RandomEnumGenerator());
      defaultProvider.AddProvider((float b) => b, new RandomFloatGenerator());
      defaultProvider.AddProvider((int b) => b, new RandomIntGenerator());
      defaultProvider.AddProvider((long b) => b, new RandomLongGenerator());
      defaultProvider.AddProvider((sbyte b) => b, new RandomSByteGenerator());
      defaultProvider.AddProvider((short b) => b, new RandomShortGenerator());
      defaultProvider.AddProvider((uint b) => b, new RandomUIntGenerator());
      defaultProvider.AddProvider((ulong b) => b, new RandomULongGenerator());
      defaultProvider.AddProvider((ushort b) => b, new RandomUShortGenerator());

      //TODO: include nullable types in the type stepping logic (key.getpreviouskey)
      defaultProvider.AddProvider((bool? b)=>b, ctx => ctx.ValueProvider.Create<bool>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((byte? b)=>b, ctx => ctx.ValueProvider.Create<byte>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((char? b)=>b, ctx => ctx.ValueProvider.Create<char>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((decimal? b)=>b, ctx => ctx.ValueProvider.Create<decimal>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((double? b)=>b, ctx => ctx.ValueProvider.Create<double>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((float? b)=>b, ctx => ctx.ValueProvider.Create<float>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((int? b)=>b, ctx => ctx.ValueProvider.Create<int>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((long? b)=>b, ctx => ctx.ValueProvider.Create<long>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((sbyte? b)=>b, ctx => ctx.ValueProvider.Create<sbyte>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((short? b)=>b, ctx => ctx.ValueProvider.Create<short>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((uint? b)=>b, ctx => ctx.ValueProvider.Create<uint>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((ulong? b)=>b, ctx => ctx.ValueProvider.Create<ulong>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((ushort? b)=>b, ctx => ctx.ValueProvider.Create<ushort>(propertyInfo: ctx.PropertyInfo));

      defaultProvider.AddProvider((string s)=>s, new RandomWordGenerator()); //string
      defaultProvider.AddProvider((DateTime dt)=>dt, new RandomPastDateTimeGenerator());

      defaultProvider.AddProvider((DateTime? dt)=>dt, ctx => ctx.ValueProvider.Create<DateTime>(propertyInfo: ctx.PropertyInfo)); 

      //constraint providers
      defaultProvider.AddProvider((string s, EmailAddressAttribute at) => s, new EmailGenerator());

      return defaultProvider;
    }

    internal CompoundValueProviderBuilder GetEmpty ()
    {
      return new CompoundValueProviderBuilder(_random);
    }
  }
}