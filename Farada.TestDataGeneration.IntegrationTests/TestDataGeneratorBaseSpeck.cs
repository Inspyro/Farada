using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using SpecK.Extensibility;
using SpecK.Specifications;
using SpecK.Specifications.InferredApi;

namespace Farada.TestDataGeneration.IntegrationTests
{
  public abstract class TestDataGeneratorBaseSpeck:Specs<DontCare>
  {
    protected Exception CreationException;
    protected TestDataDomainConfiguration TestDataDomainConfiguration { get; set; }
    protected ITestDataGenerator TestDataGenerator { get; private set; }
    protected int MaxRecursionDepth { get; private set; }

    protected Context TestDataGeneratorContext (int recursionDepth = 2, bool catchExceptions=false)
    {
        Arrangement<DontCare, DontCare> withExceptions = x =>
        {
            try
            {
                TestDataGenerator = TestDataGeneratorFactory.Create (TestDataDomainConfiguration);
            }
            catch (Exception e)
            {
                CreationException = e;
                TestDataGenerator = null;
            }
        };

        Arrangement<DontCare, DontCare> withoutException = x =>TestDataGenerator= TestDataGeneratorFactory.Create (TestDataDomainConfiguration);;

        return
                c =>
                        c.Given ("using MaxRecursionDepth of " + recursionDepth, x => MaxRecursionDepth = recursionDepth)
                                .Given ("create test data generator", catchExceptions ? withExceptions : withoutException);
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