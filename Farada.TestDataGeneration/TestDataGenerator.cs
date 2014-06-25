//TODO: Remove unnecesary dependencies

using System;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.TestDataGeneration
{
  /// <summary>
  /// Creates a test data generator for a given domain
  /// </summary>
  public static class TestDataGenerator
  {
    /// <summary>
    /// Creates a test data generator for the given domain
    /// </summary>
    /// <param name="domainConfiguration">the domain, containing all relevant information for the test data generation</param>
    /// <returns>the final compound value provider that can be used for data generation</returns>
    public static ITestDataGenerator Create (DomainConfiguration domainConfiguration)
    {
      var valueProviderBuilderFactory = new CompoundValueProviderBuilderFactory(domainConfiguration.Random);
      var valueProviderBuilder = domainConfiguration.UseDefaults ? valueProviderBuilderFactory.GetDefault() : valueProviderBuilderFactory.GetEmpty();

      if (domainConfiguration.BuildValueProvider != null)
        domainConfiguration.BuildValueProvider(valueProviderBuilder);

      return valueProviderBuilder.Build();
    }
  }
}