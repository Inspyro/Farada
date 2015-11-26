using System;
using Farada.TestDataGeneration.Modifiers;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IDomainConfigurator
  {
    ITestDataConfigurator UseDefaults (bool useDefaults);
    ITestDataConfigurator UseRandom (IRandom random);
    ITestDataConfigurator UseParameterToPropertyConversion (Func<string, string> paremeterToPropertyConversionFunc);

    ITestDataConfigurator AddInstanceModifier (IInstanceModifier instanceModifier);
  }
}