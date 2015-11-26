using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Fluent;

namespace Farada.TestDataGeneration
{
  public delegate IChainConfigurator TestDataDomainConfiguration(ITestDataConfigurator testDataConfigurator);

  /// <summary>
  /// Creates a test data generator for a given domain
  /// </summary>
  public static class TestDataGeneratorFactory
  {
    /// <summary>
    /// Creates a test data generator for the given domain
    /// </summary>
    /// <param name="testDataDomainConfiguration">the domain, containing all relevant information for the test data generation</param>
    /// <returns>the final compound value provider that can be used for data generation</returns>
    public static ITestDataGenerator Create (TestDataDomainConfiguration testDataDomainConfiguration=null)
    {
      ChainConfigurator chainConfigurator = new DomainConfigurator();

      if (testDataDomainConfiguration!=null)
      {
        chainConfigurator= (ChainConfigurator) testDataDomainConfiguration((DomainConfigurator) chainConfigurator);
      }

      return chainConfigurator.Build();
    }
  }
}