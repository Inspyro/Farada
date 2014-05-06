using System;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public class RuleSet
  {
    private readonly List<IRuleInfo> _ruleInfos;
    private readonly List<IGlobalRule> _globalRules;

    public RuleSet(params IRuleInfo[] ruleInfos)
    {
      _ruleInfos = ruleInfos == null ? new List<IRuleInfo>() : ruleInfos.ToList();
      _globalRules = new List<IGlobalRule>();
    }

    internal IEnumerable<RuleAppliance> GetRuleAppliances (Func<Type, int> getDataCount)
    {
      foreach (var ruleInfo in _ruleInfos)
      {
        var dataCountForRule = getDataCount(ruleInfo.Rule.MainDataType);

        var percentage = Math.Min(1f, (float) dataCountForRule / ruleInfo.HighDataCount);
        var executionCount = (int) (MathUtility.Lerp(ruleInfo.LowDataExecutionProbability, ruleInfo.HighDataExecutionProbability, percentage)
                             * dataCountForRule);

        yield return new RuleAppliance(ruleInfo.Rule, executionCount);
      }
    }

    internal IEnumerable<IGlobalRule> GetGlobalRules()
    {
      return _globalRules;
    }

    public void AddGlobalRule (IGlobalRule rule)
    {
      _globalRules.Add(rule);
    }
  }
}