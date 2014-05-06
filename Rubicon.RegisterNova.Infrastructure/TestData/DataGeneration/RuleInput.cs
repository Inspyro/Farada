using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public class RuleInput<T> : IRuleInput
  {
    public Handle<T> Value { get; set; }

    public Handle<T1> GetValue<T1> ()
    {
      return (Handle<T1>) ((object) Value);
    }

    public void SetValue<T1>(Handle<T1> value)
    {
      Value = (Handle<T>) ((object) value);
    }
  }

  public interface IRuleInput
  {
    Handle<T> GetValue<T> ();
    void SetValue<T> (Handle<T> value);
  }
}