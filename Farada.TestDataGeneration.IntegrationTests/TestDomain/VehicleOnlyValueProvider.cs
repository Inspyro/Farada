using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  internal class VehicleOnlyValueProvider: ValueProvider<AbstractVehicle>
  {
    protected override AbstractVehicle CreateValue (ValueProviderContext<AbstractVehicle> context)
    {
      return context.TargetValueType == typeof (AbstractVehicle.AirVehicle)
          ? (AbstractVehicle) new AbstractVehicle.AirVehicle { Engine = new AbstractVehicle.PropellorEngine () }
          : new AbstractVehicle.LandVehicle { Tire = new AbstractVehicle.Tire () };
    }
  }
}