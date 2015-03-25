using System;
using Farada.TestDataGeneration.Modifiers;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IDomainConfigurator
  {
    ITestDataConfigurator UseDefaults (bool useDefaults);
    ITestDataConfigurator UseRandom (Random random);
    ITestDataConfigurator UseParameterToPropertyConversion (Func<string, string> paremeterToPropertyConversionFunc);

    ITestDataConfigurator AddInstanceModifier (IInstanceModifier instanceModifier);
  }
}