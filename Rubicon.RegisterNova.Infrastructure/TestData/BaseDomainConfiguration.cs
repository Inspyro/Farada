using System;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  /// <summary>
  /// TODO
  /// </summary>
  public class BaseDomainConfiguration
  {
    public Random Random { get; set; }
    public Action<ICompoundValueProviderBuilder> BuildValueProvider { get; set; }

    public BaseDomainConfiguration ()
    {
      Random = new Random();
    }
  }
}