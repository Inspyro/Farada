using System;
using Farada.Core.CompoundValueProvider;

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
  }
}