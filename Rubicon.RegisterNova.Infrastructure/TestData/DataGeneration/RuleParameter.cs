using System;
using System.Linq;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public class RuleParameter<T>:IRuleParameter
  {
    private readonly Func<Handle<T>, bool> _predicate;
    private readonly T[] _excludes;


    public RuleParameter (Func<Handle<T>, bool> predicate, params T[] excludes)
    {
      _predicate = predicate;
      _excludes = excludes;
    }

    public Type DataType
    {
      get { return typeof(T); }
    }

    public object[] Excludes
    {
      get { return _excludes.Select(v => (object) v).ToArray(); }
    }

    public IRuleInput GetRuleInput (object value)
    {
      return new RuleInput<T> { Value = (Handle<T>) value };
    }

    bool IRuleParameter.Predicate (object arg)
    {
      return _predicate((Handle<T>) arg);
    }
  }

  public interface IRuleParameter
  {
    Type DataType { get; }
    object[] Excludes { get; }
    IRuleInput GetRuleInput (object value);
    bool Predicate (object arg);
  }
}