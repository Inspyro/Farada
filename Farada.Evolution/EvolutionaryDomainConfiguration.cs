using System;
using Farada.Evolution.RuleBasedDataGenerator;
using Farada.TestDataGeneration;

namespace Farada.Evolution
{
  /// <summary>
  /// TODO
  /// </summary>
  public class EvolutionaryDomainConfiguration : DomainConfiguration
  {
    public RuleSet Rules { get; set; }
  }
}