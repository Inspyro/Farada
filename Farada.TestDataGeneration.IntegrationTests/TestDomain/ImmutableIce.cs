using System;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  public class ImmutableIce
  {
    readonly string _origin;
    readonly int _temperature;

    public string Origin { get { return _origin; } }
    public int Temperature { get { return _temperature; } }

    public ImmutableIce (string origin, int temperature)
    {
      _origin = origin;
      _temperature = temperature;
    }
  }
}