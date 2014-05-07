using System;
using Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  public class DomainConfiguration:BaseDomainConfiguration
  {
    public RuleSet Rules { get; set; }
  }
}