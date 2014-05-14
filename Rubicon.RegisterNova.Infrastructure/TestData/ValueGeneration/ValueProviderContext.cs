using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration
{
  public class ValueProviderContext
  {
    public Random Random { get; set; }
    public Func<object> GetPreviousValue { get; set; }
  }
}