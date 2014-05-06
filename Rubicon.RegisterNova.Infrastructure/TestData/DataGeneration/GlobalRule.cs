using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration
{
  public abstract class GlobalRule<T>:IGlobalRule
  {
    protected abstract void Execute (Handle<T> value);

    public void Execute(object handle)
    {
      Execute((Handle<T>) handle);
    }

    public Type MainDataType
    {
      get { return typeof (T); }
    }
  }

  public interface IGlobalRule
  {
    void Execute (object handle);
    Type MainDataType { get; }
  }
}