using System;
using Farada.TestDataGeneration.CompoundValueProvider;

namespace Farada.TestDataGeneration
{
  /// <summary>
  /// TODO
  /// </summary>
  public class DomainConfiguration
  {
    public Random Random { get; set; }
    public Action<ICompoundValueProviderBuilder> BuildValueProvider { get; set; }

    public DomainConfiguration ()
    {
      Random = new Random();
    }
  }
}