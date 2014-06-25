using System;
using System.Collections.Generic;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  internal class RuleSet
  {
    private readonly List<IRule> _rules;
    private readonly List<IGlobalRule> _globalRules;

    internal RuleSet()
    {
      _rules = new List<IRule>();
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

    public void AddRule(IRule rule)
    {
      _rules.Add(rule);
    }
  }
}