using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random DateTime
  /// </summary>
  public class RandomDateTimeGenerator:ValueProvider<DateTime>
  {
    protected override DateTime CreateValue (ValueProviderContext<DateTime> context)
    {
      var start = new DateTime(1900, 1, 1);
      var end = new DateTime (2100, 1, 1);

      var range = (end - start).Days;
      return start.AddDays(context.Random.Next(range)).AddTicks(context.Random.Next(0, TimeSpan.TicksPerDay));
    }
  }
}