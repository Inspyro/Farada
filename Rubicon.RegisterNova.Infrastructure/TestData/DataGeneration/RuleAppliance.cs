using System;
using Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  internal class RuleAppliance
  {
    internal int ExecutionCount { get; private set; }
    internal IRule Rule { get;private set; }

    internal RuleAppliance(IRule rule, int executionCount)
    {
      Rule = rule;
      ExecutionCount = executionCount;
    }
  }
}