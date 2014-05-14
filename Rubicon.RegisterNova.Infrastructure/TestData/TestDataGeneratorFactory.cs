using System;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  /// <summary>
  /// TODO
  /// </summary>
  public static class TestDataGeneratorFactory
  {
    /// <summary>
    /// Creates a test data generator for the given domain
    /// </summary>
    /// <param name="baseDomainConfiguration">the domain, containing all relevant information for the test data generation</param>
    /// <param name="useDefaults">use the default generator settings (if not overriden by the domain) or use the empty settings</param>
    /// <returns>the final compound value provider that can be used for data generation</returns>
    public static ICompoundValueProvider CreateCompoundValueProvider (BaseDomainConfiguration baseDomainConfiguration, bool useDefaults = true)
    {
      var valueProviderBuilderFactory = new CompoundValueProviderBuilderFactory(baseDomainConfiguration.Random);
      var valueProviderBuilder = useDefaults ? valueProviderBuilderFactory.GetDefault() : valueProviderBuilderFactory.GetEmpty();

      if (baseDomainConfiguration.BuildValueProvider != null)
        baseDomainConfiguration.BuildValueProvider(valueProviderBuilder);

      return valueProviderBuilder.Build();
    }

    /// <summary>
    /// Creates a test data generator for the given domain
    /// </summary>
    /// <param name="domainConfiguration">the domain, containing all relevant information for the test data generation</param>
    /// <param name="useDefaults">use the default generator settings (if not overriden by the domain) or use the empty settings</param>
    /// <returns>the final test data generator that can be used for data generation</returns>
    public static RuleBasedDataGenerator.RuleBasedDataGenerator CreateRuleBasedDataGenerator (DomainConfiguration domainConfiguration, bool useDefaults = true)
    {
      var valueProvider = CreateCompoundValueProvider(domainConfiguration, useDefaults);
      return new RuleBasedDataGenerator.RuleBasedDataGenerator(valueProvider, domainConfiguration.Random, domainConfiguration.Rules);
    }
  }
}