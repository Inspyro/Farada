using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  internal class VehicleOnlyValueProvider: ValueProvider<AbstractVehicle>
  {
    protected override AbstractVehicle CreateValue (ValueProviderContext<AbstractVehicle> context)
    {
      return context.TargetValueType == typeof (AirVehicle)
          ? (AbstractVehicle) new AirVehicle { Engine = new PropellorEngine () }
          : new LandVehicle { Tire = new Tire () };
    }
  }
}