using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class VehicleSubTypeProvider:SubTypeValueProvider<AbstractVehicle>
  {
    protected override AbstractVehicle CreateValue (ValueProviderContext<AbstractVehicle> context)
    {
      if(context.TargetValueType==typeof(AirVehicle))
      {
        return new AirVehicle { Engine = new JetEngine { FuelUsePerSecond = 20 } };
      }
      
      if(context.TargetValueType==typeof(LandVehicle))
      {
        return new LandVehicle { Tire = new Tire { Diameter = 10 } };
      }

      throw new InvalidOperationException ("property of type " + context.TargetValueType + " is not supported by " + this.GetType ().FullName);
    }
  }
}