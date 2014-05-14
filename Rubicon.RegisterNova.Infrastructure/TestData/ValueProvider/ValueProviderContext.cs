using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  public class ValueProviderContext // TODO: Make generic.
  {
    public Random Random { get; private set; }
    public Func<object> GetPreviousValue { get; private set; }

    internal ValueProviderContext(Random random, Func<object> getPreviousValueFunc)
    {
      Random = random;
      GetPreviousValue = getPreviousValueFunc;
    }
  }
}