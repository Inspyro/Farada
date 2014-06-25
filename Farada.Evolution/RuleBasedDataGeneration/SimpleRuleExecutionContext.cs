using System;
using Farada.TestDataGeneration.CompoundValueProviders;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public class SimpleRuleExecutionContext<T>:RuleExecutionContext
  {
    public RuleValue<T> Input { get; private set; }

    internal SimpleRuleExecutionContext (ITestDataGenerator testDataGenerator, IReadableWorld world, RuleValue<T> input)
        : base(testDataGenerator, world)
    {
      Input = input;
    }
  }
}