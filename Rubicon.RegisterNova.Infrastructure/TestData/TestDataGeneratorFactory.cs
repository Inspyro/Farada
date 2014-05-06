using System;
using Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  public class TestDataGeneratorFactory
  {
    private readonly RuleSet _ruleSet;
    public ChainValueProviderBuilderFactory ValueProviderBuilderFactory { get; private set; }

    public TestDataGeneratorFactory(RandomGeneratorProvider randomGeneratorProvider, RuleSet ruleSet=null)
    {
      ValueProviderBuilderFactory = new ChainValueProviderBuilderFactory(randomGeneratorProvider);
      _ruleSet = ruleSet;
    }

    public TestDataGenerator Build (ChainValueProviderBuilder valueProviderBuilder)
    {
      return new TestDataGenerator(new TypeValueProvider(valueProviderBuilder.ToValueProvider()), _ruleSet);
    }
  }
}
