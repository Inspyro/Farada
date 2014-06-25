using System;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.TestDataGeneration
{
  /// <summary>
  /// Describes the domain used for generating test data
  /// </summary>
  public class DomainConfiguration
  {
    /// <summary>
    /// The random to use everywhere in the data generation (useful if you want to use a specific seed)
    /// </summary>
    public Random Random { get; set; }

    /// <summary>
    /// Default: True means that value providers for basic types are injected into the test data generator, false means the domain stays empty
    /// </summary>
    public bool UseDefaults { get; set; }

    /// <summary>
    /// The action that sets up the <see cref="ICompoundValueProviderBuilder"/> and so the provider chains in the domain
    /// </summary>
    public Action<ICompoundValueProviderBuilder> BuildValueProvider { get; set; }

    public DomainConfiguration ()
    {
      Random = new Random();
      UseDefaults = true;
    }
  }
}