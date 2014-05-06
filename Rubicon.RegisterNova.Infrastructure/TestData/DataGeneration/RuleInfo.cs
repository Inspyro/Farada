using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public class RuleInfo<T>:IRuleInfo
  {
    public IRule Rule { get; private set; }
    public float LowDataExecutionProbability { get; private set; }
    public float HighDataExecutionProbability { get; private set; }
    public int HighDataCount { get; private set; }

    public Type MainDataType
    {
      get { return Rule.MainDataType; }
    }

    /// <summary>
    /// Create a new rule info with a given execution probability, not depending on the data size
    /// </summary>
    /// <param name="rule">the rule to apply</param>
    /// <param name="executionProbability">the probability of the rule to be executed per value</param>
    public RuleInfo (Rule<T> rule, float executionProbability)
        : this(rule, executionProbability, executionProbability, 0)
    {
    }

    /// <summary>
    /// Creates a new rule info with a given low and high probability
    /// </summary>
    /// <param name="rule">the rule to apply</param>
    /// <param name="lowDataExecutionProbability">the probability that the rule is executed when there is only a small amount of data</param>
    /// <param name="highDataExecutionProbability">the probability that the rule is executed when there is a high amount of data (>= value specified as highDataCount)</param>
    /// <param name="highDataCount">the data count that has to be reached to use the highDataExecutionProbability (linear interpolation from low->high data count)</param>
    public RuleInfo(Rule<T> rule, float lowDataExecutionProbability, float highDataExecutionProbability, int highDataCount)
    {
      Rule = rule;
      LowDataExecutionProbability = lowDataExecutionProbability;
      HighDataExecutionProbability = highDataExecutionProbability;
      HighDataCount = highDataCount;
    }
  }

  public interface IRuleInfo
  {
    IRule Rule { get; }
    float LowDataExecutionProbability { get; }
    float HighDataExecutionProbability { get; }
    int HighDataCount { get; }
  }
}