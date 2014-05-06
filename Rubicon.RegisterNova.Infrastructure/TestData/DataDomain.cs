using System;
using Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  public class DataDomain
  {
    public Random Random { get; set; }
    public RuleSet Rules { get; set; }
    public Action<RandomGeneratorProvider> SetupRandomProviderAction { get; set; }
    public Action<ChainValueProviderBuilder> SetupValueProviderAction { get; set; }

    public DataDomain()
    {
      Random = new Random();
    }
  }
}