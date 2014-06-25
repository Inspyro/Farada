using System;
using Farada.Evolution.RuleBasedDataGeneration;
using Farada.TestDataGeneration;

namespace Farada.Evolution
{
  /// <summary>
  /// TODO
  /// </summary>
  public static class EvolutionaryDataGenerator
  {
    /// <summary>
    /// Creates an evolutionary data generator for the given domain
    /// </summary>
    /// <param name="evolutionaryDomainConfiguration">the domain, containing all relevant information for the test data generation</param>
    /// <param name="useDefaults">use the default generator settings (if not overriden by the domain) or use the empty settings</param>
    /// <returns>the final test data generator that can be used for data generation</returns>
    public static RuleBasedDataGenerator CreateRuleBasedDataGenerator (EvolutionaryDomainConfiguration evolutionaryDomainConfiguration, bool useDefaults = true)
    {
      var testDataGenerator = TestDataGenerator.Create(evolutionaryDomainConfiguration, useDefaults);
      return new RuleBasedDataGenerator(testDataGenerator, evolutionaryDomainConfiguration.Random, evolutionaryDomainConfiguration.Rules);
    }
  }
}