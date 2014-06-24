﻿using System;

namespace Farada.Evolution
{
  /// <summary>
  /// TODO
  /// </summary>
  public class Farada: Core.Farada
  {
    /// <summary>
    /// Creates a test data generator for the given domain
    /// </summary>
    /// <param name="evolutionaryDomainConfiguration">the domain, containing all relevant information for the test data generation</param>
    /// <param name="useDefaults">use the default generator settings (if not overriden by the domain) or use the empty settings</param>
    /// <returns>the final test data generator that can be used for data generation</returns>
    public static RuleBasedDataGenerator.RuleBasedDataGenerator CreateRuleBasedDataGenerator (EvolutionaryDomainConfiguration evolutionaryDomainConfiguration, bool useDefaults = true)
    {
      var valueProvider = CreateCompoundValueProvider(evolutionaryDomainConfiguration, useDefaults);
      return new RuleBasedDataGenerator.RuleBasedDataGenerator(valueProvider, evolutionaryDomainConfiguration.Random, evolutionaryDomainConfiguration.Rules);
    }
  }
}