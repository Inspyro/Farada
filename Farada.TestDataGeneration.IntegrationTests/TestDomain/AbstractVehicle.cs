using System;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  abstract class AbstractVehicle
  {
    [InitialValueForChain(50)]
    public int Weight { get; set; }

    [InitialStringValueForChain("!")]
    public string Name { get; set; }

    public Color MainColor { get; set; }

    internal class LandVehicle : AbstractVehicle
    {
      public Tire Tire { get; set; }
    }

    internal class Tire
    {
      public double Diameter { get; set; }
      public double Grip { get; set; }

      [InitialValueForChain(100)]
      public double TireUsage { get; set; }
    }

    internal class AirVehicle : AbstractVehicle
    {
      public Engine Engine { get; set; }
    }

    internal class JetEngine : Engine
    {
      public float FuelUsePerSecond { get; set; }
    }

    internal class PropellorEngine:Engine
    {
      public float AverageRotationSpeed { get;set;}
    }

    internal abstract class Engine
    {
      public float PowerInNewtons { get;set; }
    }

    internal enum Color
    {
      White,
      Green,
      Red,
      Black
    }

    internal class InitialValueForChainAttribute : Attribute
    {
      public int BaseValue { get; private set; }

      public InitialValueForChainAttribute (int baseValue)
      {
        BaseValue = baseValue;
      }
    }

    internal class InitialStringValueForChainAttribute : Attribute
    {
      public string BaseValue { get; private set; }

      public InitialStringValueForChainAttribute (string baseValue)
      {
        BaseValue = baseValue;
      }
    }
  }
}