using System;
using System.Linq;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public class RuleParameter<T>:IRuleParameter
  {
    private readonly Func<RuleValue<T>, bool> _predicate;
    private readonly Func<RuleValue<T>, CompoundRuleInput, int[]> _getUnwantedParameterIndices;

    public RuleParameter (Func<RuleValue<T>, bool> predicate=null, Func<RuleValue<T>, CompoundRuleInput, int[]> getUnwantedParameterIndices=null)
    {
      _predicate = predicate;
      _getUnwantedParameterIndices = getUnwantedParameterIndices;
    }

    public Type DataType
    {
      get { return typeof(T); }
    }

    bool IRuleParameter.Predicate (IRuleValue arg)
    {
      return (_predicate == null || _predicate((RuleValue<T>) arg));
    }

    int[] IRuleParameter.ParameterPredicate(IRuleValue arg, CompoundRuleInput targetInput)
    {
      return _getUnwantedParameterIndices == null ? null : _getUnwantedParameterIndices((RuleValue<T>) arg, targetInput);
    }
  }

  public interface IRuleParameter
  {
    Type DataType { get; }
    bool Predicate (IRuleValue arg);
    int[] ParameterPredicate (IRuleValue arg, CompoundRuleInput targetInput);
  }
}