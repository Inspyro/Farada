using System;
using Rubicon.RegisterNova.Infrastructure.TestData.HelperCode;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  public class TestDataGeneratorFactory
  {
    public ChainValueProviderBuilderFactory ValueProviderBuilderFactory { get; private set; }

    public TestDataGeneratorFactory(RandomGeneratorProvider randomGeneratorProvider)
    {
      ValueProviderBuilderFactory = new ChainValueProviderBuilderFactory(randomGeneratorProvider);
    }

    public TestDataGenerator Build (ChainValueProviderBuilder valueProviderBuilder)
    {
      return new TestDataGenerator(new TypeValueProvider(valueProviderBuilder.ToValueProvider()));
    }
  }
}
