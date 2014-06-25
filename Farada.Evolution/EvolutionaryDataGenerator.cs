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
    /// <returns>the final test data generator that can be used for data generation</returns>
    public static RuleBasedDataGenerator CreateRuleBasedDataGenerator (EvolutionaryDomainConfiguration evolutionaryDomainConfiguration)
    {
      var testDataGenerator = TestDataGenerator.Create(evolutionaryDomainConfiguration);
      return new RuleBasedDataGenerator(testDataGenerator, evolutionaryDomainConfiguration.Random, evolutionaryDomainConfiguration.Rules);
    }
  }
}