using System;
using System.Linq;

namespace Farada.Evolution.RuleBasedDataGenerator
{
  public class RuleParameter<T>:IRuleParameter
  {
    private readonly Func<RuleValue<T>, bool> _predicate;
    private readonly T[] _excludes;

    public RuleParameter (Func<RuleValue<T>, bool> predicate=null, params T[] excludes)
    {
      _predicate = predicate;
      _excludes = excludes;
    }

    public Type DataType
    {
      get { return typeof(T); }
    }

    public IRuleValue[] Excludes
    {
      get { return _excludes.Select(v => (IRuleValue) v).ToArray(); }
    }

    bool IRuleParameter.Predicate (IRuleValue arg)
    {
      return _predicate==null||_predicate((RuleValue<T>) arg);
    }
  }

  public interface IRuleParameter
  {
    Type DataType { get; }
    IRuleValue[] Excludes { get; }
    bool Predicate (IRuleValue arg);
  }
}