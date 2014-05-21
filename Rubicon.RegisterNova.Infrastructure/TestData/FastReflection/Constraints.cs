using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.FastReflection
{
  public class Constraints
  {
    public int? MinLength { get; internal set; }
    public int? MaxLength { get; internal set; }

    public int? MinIntRange { get; internal set; }
    public int? MaxIntRange { get; internal set; }
    public double? MinDoubleRange { get; internal set; }
    public double? MaxDoubleRange { get; internal set; }
  }
}