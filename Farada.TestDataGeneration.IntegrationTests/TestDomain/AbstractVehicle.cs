using System;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  abstract class AbstractVehicle
  {
    [InitialValueForChain (50)]
    public int Weight { get; set; }

    [InitialStringValueForChain ("!")]
    public string Name { get; set; }

    public Color MainColor { get; set; }
  }
}

class LandVehicle : AbstractVehicle
{
  public Tire Tire { get; set; }
}

class Tire
{
  public double Diameter { get; set; }
  public double Grip { get; set; }

  [InitialValueForChain (100)]
  public double TireUsage { get; set; }
}

class AirVehicle : AbstractVehicle
{
  public Engine Engine { get; set; }
}

class CustomVehicle :AbstractVehicle
{
  public Vector? CurrentDirection;
}

interface IVector
{
  
}
struct Vector : IVector
{
  public string Id { get; set; }
}

class JetEngine : Engine
{
  public float FuelUsePerSecond { get; set; }
}

class PropellorEngine : Engine
{
  public float AverageRotationSpeed { get; set; }
}

abstract class Engine
{
  public float PowerInNewtons { get; set; }
}

enum Color
{
  White,
  Green,
  Red,
  Black
}

class InitialValueForChainAttribute : Attribute
{
  public int BaseValue { get; private set; }

  public InitialValueForChainAttribute (int baseValue)
  {
    BaseValue = baseValue;
  }
}

class InitialStringValueForChainAttribute : Attribute
{
  public string BaseValue { get; private set; }

  public InitialStringValueForChainAttribute (string baseValue)
  {
    BaseValue = baseValue;
  }
}