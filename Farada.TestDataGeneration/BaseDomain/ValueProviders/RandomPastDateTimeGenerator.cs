using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random DateTime
  /// </summary>
  public class RandomPastDateTimeGenerator:ValueProvider<DateTime>
  {
    protected override DateTime CreateValue (ValueProviderContext<DateTime> context)
    {
      var start = new DateTime(1900, 1, 1);

      var range = (DateTime.Today - start).Days; //TODO: WTF?
      return start.AddDays(context.Random.Next(range)).AddTicks(context.Random.Next(0, TimeSpan.TicksPerDay));
    }
  }
}