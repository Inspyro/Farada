using System;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;

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

      throw new InvalidOperationException ("property of type " + context.TargetValueType + " is not supported by " + GetType ().FullName);
    }

    //We indicate that we fill all types with values, so they are not automatically filled by farada anymore.
    public override ValueFillMode FillMode { get { return ValueFillMode.FillAll; } }
  }
}