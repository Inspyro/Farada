using System;
using Farada.Core;
using Farada.Evolution.RuleBasedDataGenerator;

namespace Farada.Evolution
{
  /// <summary>
  /// TODO
  /// </summary>
  public class DomainConfiguration : BaseDomainConfiguration
  {
    public RuleSet Rules { get; set; }
  }
}