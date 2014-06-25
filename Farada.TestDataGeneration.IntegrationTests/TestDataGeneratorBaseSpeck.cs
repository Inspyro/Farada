using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using SpecK.Extensibility;
using SpecK.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
  public abstract class TestDataGeneratorBaseSpeck:Specs<DontCare>
  {
    protected DomainConfiguration Domain { get; set; }
    protected ITestDataGenerator TestDataGenerator { get; private set; }
    protected int MaxRecursionDepth { get; private set; }

    protected Context TestDataGeneratorContext (int recursionDepth = 2)
    {
      return
          c =>
              c.Given ("using MaxRecursionDepth of " + recursionDepth, x => MaxRecursionDepth = recursionDepth)
                  .Given ("create compound value provider",
                      x => TestDataGenerator = TestDataGeneration.TestDataGenerator.Create (Domain));
    }

    protected Context BaseDomainContext (bool useDefaults = true, int? seed = null)
    {
      return c => c
          .Given ("empty base domain with seed " + (!seed.HasValue ? "any" : seed.ToString ()),
              x => Domain = new DomainConfiguration { UseDefaults = useDefaults, Random = seed.HasValue ? new Random (seed.Value) : new Random () })
          .Given (TestDataGeneratorContext ());
    }
  }
}