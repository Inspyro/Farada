using System;
using Farada.Core.CompoundValueProvider;

namespace Farada.Core
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