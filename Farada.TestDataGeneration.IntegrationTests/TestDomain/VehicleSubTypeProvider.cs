using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class VehicleSubTypeProvider:SubTypeValueProvider<AbstractVehicle>
  {
    protected override AbstractVehicle CreateValue (ValueProviderContext<AbstractVehicle> context)
    {
      if(context.TargetValueType==typeof(AbstractVehicle.AirVehicle))
      {
        return new AbstractVehicle.AirVehicle { Engine = new AbstractVehicle.JetEngine { FuelUsePerSecond = 20 } };
      }
      
      if(context.TargetValueType==typeof(AbstractVehicle.LandVehicle))
      {
        return new AbstractVehicle.LandVehicle { Tire = new AbstractVehicle.Tire { Diameter = 10 } };
      }

      throw new InvalidOperationException ("property of type " + context.TargetValueType + " is not supported by " + this.GetType ().FullName);
    }
  }
}