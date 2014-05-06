using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  public static class TestDataGeneratorFacade
  {
    /// <summary>
    /// Creates a test data generator for the given domain
    /// </summary>
    /// <param name="domain">the domain, containing all relevant information for the test data generation</param>
    /// <param name="useDefaults">use the default generator settings (if not overriden by the domain) or use the empty settings</param>
    /// <returns>the final test data generator that can be used for data generation</returns>
    public static TestDataGenerator Get(Domain domain, bool useDefaults=true)
    {
      var randomProviderFactory = new RandomGeneratorProviderFactory(domain.Random);
      var randomProvider = useDefaults ? randomProviderFactory.GetDefault() : randomProviderFactory.GetEmpty();

      if(domain.SetupRandomProviderAction!=null)
      {
        domain.SetupRandomProviderAction(randomProvider);
      }

      var testDataGeneratorFactory = new TestDataGeneratorFactory(randomProvider, domain.Rules);

      var valueProvider = useDefaults
          ? testDataGeneratorFactory.ValueProviderBuilderFactory.GetDefault()
          : testDataGeneratorFactory.ValueProviderBuilderFactory.GetEmpty();

      if(domain.SetupValueProviderAction!=null)
      {
        domain.SetupValueProviderAction(valueProvider);
      }

      return testDataGeneratorFactory.Build(valueProvider);
    }
  }
}
