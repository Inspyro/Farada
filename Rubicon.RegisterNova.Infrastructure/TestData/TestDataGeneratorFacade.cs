using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  public static class TestDataGeneratorFacade
  {
    /// <summary>
    /// Creates a test data generator for the given domain
    /// </summary>
    /// <param name="dataDomain">the domain, containing all relevant information for the test data generation</param>
    /// <param name="useDefaults">use the default generator settings (if not overriden by the domain) or use the empty settings</param>
    /// <returns>the final test data generator that can be used for data generation</returns>
    public static TestDataGenerator Get(DataDomain dataDomain, bool useDefaults=true)
    {
      var randomProviderFactory = new RandomGeneratorProviderFactory(dataDomain.Random);
      var randomProvider = useDefaults ? randomProviderFactory.GetDefault() : randomProviderFactory.GetEmpty();

      if(dataDomain.SetupRandomProviderAction!=null)
      {
        dataDomain.SetupRandomProviderAction(randomProvider);
      }

      var testDataGeneratorFactory = new TestDataGeneratorFactory(randomProvider, dataDomain.Rules);

      var valueProvider = useDefaults
          ? testDataGeneratorFactory.ValueProviderBuilderFactory.GetDefault()
          : testDataGeneratorFactory.ValueProviderBuilderFactory.GetEmpty();

      if(dataDomain.SetupValueProviderAction!=null)
      {
        dataDomain.SetupValueProviderAction(valueProvider);
      }

      return testDataGeneratorFactory.Build(valueProvider);
    }
  }
}
