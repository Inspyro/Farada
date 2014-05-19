using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  internal class RandomPastDateTimeGenerator:ValueProvider<DateTime>
  {
    protected override DateTime CreateValue (ValueProviderContext<DateTime> context)
    {
      var start = new DateTime(1900, 1, 1);

      var range = (DateTime.Today - start).Days;
      return start.AddDays(context.Random.Next(range)).AddTicks((long) 24 * 60 * 60 * 1000 * 1000);
    }
  }
}