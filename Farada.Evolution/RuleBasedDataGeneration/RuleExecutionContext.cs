using System;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public class RuleExecutionContext
  {
    public ITestDataGenerator TestDataGenerator { get; private set; }
    public IReadableWorld World { get; private set; }

    internal RuleExecutionContext(ITestDataGenerator testDataGenerator, IReadableWorld world)
    {
      TestDataGenerator = testDataGenerator;
      World = world;
    }
  }
}