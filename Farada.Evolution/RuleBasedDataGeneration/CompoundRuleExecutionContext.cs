using System;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public class CompoundRuleExecutionContext:RuleExecutionContext
  {
    public CompoundRuleInput InputData { get; private set; }

    internal CompoundRuleExecutionContext (CompoundRuleInput inputData, ITestDataGenerator testDataGenerator, IReadableWorld world)
        : base(testDataGenerator, world)
    {
      InputData = inputData;
    }
  }
}