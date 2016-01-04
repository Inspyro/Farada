using System;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.Modifiers;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Fluent
{
  public interface IDomainConfigurator
  {
    ITestDataConfigurator UseDefaults (bool useDefaults);
    ITestDataConfigurator UseRandom (IRandom random);
    ITestDataConfigurator UseParameterToPropertyConversion (Func<string, string> paremeterToPropertyConversionFunc);

    ITestDataConfigurator UseMemberExtensionService (IMemberExtensionService memberExtensionService);

    ITestDataConfigurator AddInstanceModifier (IInstanceModifier instanceModifier);
  }
}