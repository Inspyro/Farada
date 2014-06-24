using System;
using Farada.Core.CompoundValueProvider;

//TODO: Remove unnecesary dependencies
namespace Farada.Core
{
  /// <summary>
  /// TODO
  /// </summary>
  public class Farada
  {
    /// <summary>
    /// Creates a test data generator for the given domain
    /// </summary>
    /// <param name="domainConfiguration">the domain, containing all relevant information for the test data generation</param>
    /// <param name="useDefaults">use the default generator settings (if not overriden by the domain) or use the empty settings</param>
    /// <returns>the final compound value provider that can be used for data generation</returns>
    /// 
    /// //TODO: move useDefaults to base domain configuration
    public static ICompoundValueProvider CreateCompoundValueProvider (DomainConfiguration domainConfiguration, bool useDefaults = true)
    {
      var valueProviderBuilderFactory = new CompoundValueProviderBuilderFactory(domainConfiguration.Random);
      var valueProviderBuilder = useDefaults ? valueProviderBuilderFactory.GetDefault() : valueProviderBuilderFactory.GetEmpty();

      if (domainConfiguration.BuildValueProvider != null)
        domainConfiguration.BuildValueProvider(valueProviderBuilder);

      return valueProviderBuilder.Build();
    }
  }
}