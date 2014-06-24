using System;
using Farada.Core;
using Farada.Evolution.RuleBasedDataGenerator;

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