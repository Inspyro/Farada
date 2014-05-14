using System;
using Rubicon.RegisterNova.Infrastructure.TestData.RuleBasedDataGenerator;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  /// <summary>
  /// TODO
  /// </summary>
  public class DomainConfiguration : BaseDomainConfiguration
  {
    public RuleSet Rules { get; set; }
  }
}