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

      //TODO: include nullable types in the type stepping logic (key.getpreviouskey)
      defaultProvider.AddProvider((bool? b)=>b, ctx => ctx.ValueProvider.Create<bool>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((byte? b)=>b, ctx => ctx.ValueProvider.Create<byte>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((char? c)=>c, ctx => ctx.ValueProvider.Create<char>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((decimal? d)=>d, ctx => ctx.ValueProvider.Create<decimal>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((double? d)=>d, ctx => ctx.ValueProvider.Create<double>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((float? f)=>f, ctx => ctx.ValueProvider.Create<float>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((int? i)=>i, ctx => ctx.ValueProvider.Create<int>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((long? l)=>l, ctx => ctx.ValueProvider.Create<long>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((sbyte? sb)=>sb, ctx => ctx.ValueProvider.Create<sbyte>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((short? s)=>s, ctx => ctx.ValueProvider.Create<short>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((uint? ui)=>ui, ctx => ctx.ValueProvider.Create<uint>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((ulong? ul)=>ul, ctx => ctx.ValueProvider.Create<ulong>(propertyInfo: ctx.PropertyInfo));
      defaultProvider.AddProvider((ushort? us)=>us, ctx => ctx.ValueProvider.Create<ushort>(propertyInfo: ctx.PropertyInfo));

      defaultProvider.AddProvider((string s)=>s, new RandomWordGenerator()); //string
      defaultProvider.AddProvider((DateTime dt)=>dt, new RandomPastDateTimeGenerator());

      defaultProvider.AddProvider((DateTime? dt)=>dt, ctx => ctx.ValueProvider.Create<DateTime>(propertyInfo: ctx.PropertyInfo)); 

      //constraint providers
      defaultProvider.AddProvider((string s, EmailAddressAttribute eaa) => s, new EmailGenerator());

      return defaultProvider;
    }

    internal CompoundValueProviderBuilder GetEmpty ()
    {
      return new CompoundValueProviderBuilder(_random);
    }
  }
}