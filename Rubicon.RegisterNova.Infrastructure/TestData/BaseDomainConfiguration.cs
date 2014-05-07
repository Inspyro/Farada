using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  public class BaseDomainConfiguration
  {
    public Random Random { get; set; }
    public Action<CompoundValueProviderBuilder> BuildValueProvider { get; set; }

    public BaseDomainConfiguration()
    {
      Random = new Random();
    }
  }
}