using System;
using Farada.Core.CompoundValueProvider;

namespace Farada.Core
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