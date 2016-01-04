using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.ValueProviders
{
  [Subject(typeof(RangedValueProvider<>), "Create")]
  public class RangedValueProviderSpec : Spec
  {
    static TestDataDomainConfiguration TestDataDomainConfiguration;
    static ITestDataGenerator TestDataGenerator;

    public RangedValueProviderSpec()
    {
      Specify (x => TestDataGenerator.Create<ClassWithDateRange> ())
          .Case ("when creating class with date range", _ => _
              .Given (BaseContext (seed: 5))
              .It ("returns date in between", x => x.Result.BirthDate.ToShortDateString().Should ().Be ("22.05.2013")));
    }

    Context BaseContext(int seed)
    {
      return
          c => c
              .Given("Empty domain with date range value provider", x =>
                 TestDataDomainConfiguration = (context => context
                     .UseDefaults(false)
                     .UseRandom(new DefaultRandom(seed))
                     .For<ClassWithDateRange>().AddProvider(new DefaultInstanceValueProvider<ClassWithDateRange>())
                     .For<ClassWithDateRange>().Select(cwdr=>cwdr.BirthDate).AddProvider(new DateGenerator())))
              .Given("TestDataGenerator", x => TestDataGenerator = TestDataGeneratorFactory.Create(TestDataDomainConfiguration));
    }
  }

  class DateGenerator : AttributeBasedRangedValueProvider<DateTime, DateRangeAttribute>
  {
    protected override DateTime CreateValue (RangedValueProviderContext<DateTime> context)
    {
      return
          context.RangeConstraints.From.AddTicks (
              (long) ((context.RangeConstraints.To - context.RangeConstraints.From).Ticks * context.Random.NextDouble ()));
    }

    protected override GenericRangeConstraints<DateTime> CreateConstraints (DateRangeAttribute attribute)
    {
      return new GenericRangeConstraints<DateTime> (DateTime.Parse(attribute.From), DateTime.Parse(attribute.To));
    }
  }

  public class DateRangeAttribute : Attribute
  {
    public string From { get; set; }
    public string To { get; set; }
  }

  public class ClassWithDateRange
  {
    [DateRange (From = "21.05.2013", To = "25.05.2013")]
    public DateTime BirthDate { get; set; }
  }
}