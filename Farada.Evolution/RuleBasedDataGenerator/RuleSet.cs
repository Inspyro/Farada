using System;
using System.Collections.Generic;
using System.Linq;

namespace Farada.Evolution.RuleBasedDataGenerator
{
  public class RuleSet
  {
    private readonly List<IRule> _rules;
    private readonly List<IGlobalRule> _globalRules;

    public RuleSet(params IRule[] ruleInfos)
    {
      _rules = ruleInfos == null ? new List<IRule>() : ruleInfos.ToList();
      _globalRules = new List<IGlobalRule>();
    }

    internal IEnumerable<IRule> GetRules()
    {
      return _rules;
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