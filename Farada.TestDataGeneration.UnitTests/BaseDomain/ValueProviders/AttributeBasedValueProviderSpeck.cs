using System;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.ValueProviders
{
  [Subject(typeof(AttributeBasedValueProvider<,>), nameof(AttributeBasedValueProvider<string,DateRangeAttribute>))]
  public class AttributeBasedValueProviderSpec : Spec
  {
    static TestDataDomainConfiguration TestDataDomainConfiguration;
    static ITestDataGenerator TestDataGenerator;

    public AttributeBasedValueProviderSpec()
    {
      Specify(x => TestDataGenerator.Create<ClassWithMultipleAttributes>())
          .Case("when creating class with multiple attributes", _ => _
             .Given(BaseContext(seed: 5))
             .It("returns date in between", x => x.Result.Country.Should().Be("Vienna,Graz")));
    }

    Context BaseContext(int seed)
    {
      return
          c => c
              .Given("Empty domain with attribute based value provider", x =>
                 TestDataDomainConfiguration = (context => context
                     .UseDefaults(false)
                     .UseRandom(new DefaultRandom(seed))
                     .For<ClassWithMultipleAttributes>().AddProvider(new DefaultInstanceValueProvider<ClassWithMultipleAttributes>())
                     .For<ClassWithMultipleAttributes>().Select(cwma => cwma.Country).AddProvider(new CountryCityProvider())))
              .Given("TestDataGenerator", x => TestDataGenerator = TestDataGeneratorFactory.Create(TestDataDomainConfiguration));
    }
  }

  class CountryCityProvider : AttributeBasedValueProvider<string, CityAttribute>
  {
    protected override string CreateAttributeBasedValue (AttributeValueProviderContext<string, CityAttribute> context)
    {
      return string.Join (",", context.Attributes.Select (a => a.Name));
    }
  }

  [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
  public class CityAttribute : Attribute
  {
    public string Name { get; set; }
  }

  public class ClassWithMultipleAttributes
  {
    [City(Name = "Graz")]
    [City(Name = "Vienna")]
    public string Country { get; set; }
  }
}