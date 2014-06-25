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

    /// <summary>
    /// Default: True means that value providers for basic types are injected into the test data generator, false means the domain stays empty
    /// </summary>
    public bool UseDefaults { get; set; }

    public Action<ICompoundValueProviderBuilder> BuildValueProvider { get; set; }

    public DomainConfiguration ()
    {
      Random = new Random();
      UseDefaults = true;
    }
  }
}