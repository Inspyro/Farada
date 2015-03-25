using System;
using Farada.Evolution.RuleBasedDataGeneration;
using Farada.TestDataGeneration;
using JetBrains.Annotations;

namespace Farada.Evolution
{
  public delegate IRuleBasedDataGeneratorConfigurator EvolutionaryDomainConfiguration(IRuleBasedDataGeneratorConfigurator rulbeBasedConfigurator);

  public interface IRuleBasedDataGeneratorConfigurator
  {
    IRuleBasedDataGeneratorConfigurator AddGlobalRule (IGlobalRule rule);
    IRuleBasedDataGeneratorConfigurator AddRule (IRule rule);
  }

  internal class RuleBasedDataGeneratorConfigurator:IRuleBasedDataGeneratorConfigurator
  {
    private readonly RuleSet _ruleSet;

    internal RuleBasedDataGeneratorConfigurator()
    {
      _ruleSet = new RuleSet();
    }

    public IRuleBasedDataGeneratorConfigurator AddGlobalRule (IGlobalRule rule)
    {
      _ruleSet.AddGlobalRule(rule);
      return this;
    }

    public IRuleBasedDataGeneratorConfigurator AddRule (IRule rule)
    {
      _ruleSet.AddRule(rule);
      return this;
    }

    public RuleSet Build ()
    {
      return _ruleSet;
    }
  }

  /// <summary>
  /// Creates an evolutionary data generator for the given domain
  /// </summary>
  public static class EvolutionaryDataGeneratorFactory
  {
    /// <summary>
    /// Creates an evolutionary data generator for the given domain
    /// </summary>
    /// <param name="testDataDomainConfiguration">the domain, containing all relevant imformation for the test data generation</param>
    /// <param name="evolutionaryDomainConfiguration">the domain, containing all relevant information for the evolutionary generation</param>
    /// <returns>the final test data generator that can be used for data generation</returns>
    public static RuleBasedDataGenerator Create (TestDataDomainConfiguration testDataDomainConfiguration, [CanBeNull] EvolutionaryDomainConfiguration evolutionaryDomainConfiguration)
    {
      var testDataGenerator = TestDataGeneratorFactory.Create(testDataDomainConfiguration);

      var evolutionaryDomainConfigurator = new RuleBasedDataGeneratorConfigurator();
      if (evolutionaryDomainConfiguration != null)
      {
        evolutionaryDomainConfigurator = (RuleBasedDataGeneratorConfigurator) evolutionaryDomainConfiguration(evolutionaryDomainConfigurator);
      }

      var ruleSet = evolutionaryDomainConfigurator.Build();
      return new RuleBasedDataGenerator(testDataGenerator, ruleSet);
    }
  }
}