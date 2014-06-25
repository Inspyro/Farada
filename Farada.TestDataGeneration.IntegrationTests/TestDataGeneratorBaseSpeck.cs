using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using SpecK.Extensibility;
using SpecK.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
  public abstract class TestDataGeneratorBaseSpeck:Specs<DontCare>
  {
    protected TestDataDomainConfiguration TestDataDomainConfiguration { get; set; }
    protected ITestDataGenerator TestDataGenerator { get; private set; }
    protected int MaxRecursionDepth { get; private set; }

    protected Context TestDataGeneratorContext (int recursionDepth = 2)
    {
      return
          c =>
              c.Given ("using MaxRecursionDepth of " + recursionDepth, x => MaxRecursionDepth = recursionDepth)
                  .Given ("create test data generator",
                      x => TestDataGenerator = TestDataGeneratorFactory.Create (TestDataDomainConfiguration));
    }

    protected Context BaseDomainContext (bool useDefaults = true, int? seed = null)
    {
      return c => c
          .Given ("empty base domain with seed " + (!seed.HasValue ? "any" : seed.ToString ()),
              x => TestDataDomainConfiguration=(configurator => configurator.UseDefaults(useDefaults).UseRandom(seed.HasValue ? new Random (seed.Value) : new Random ())))
          .Given (TestDataGeneratorContext ());
    }
  }
}