using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;
using TestFx.SpecK;
using TestFx.SpecK.InferredApi;

namespace Farada.TestDataGeneration.IntegrationTests
{
  public abstract class TestDataGeneratorBaseSpeck : Spec
  {
    protected Exception CreationException;
    protected TestDataDomainConfiguration TestDataDomainConfiguration { get; set; }
    protected ITestDataGenerator TestDataGenerator { get; private set; }
    protected int MaxRecursionDepth { get; private set; }

    protected Context TestDataGeneratorContext (int recursionDepth = 2, bool catchExceptions = false)
    {
      Arrangement<Dummy, Dummy, Dummy, Dummy> withExceptions = x =>
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

      Arrangement<Dummy, Dummy, Dummy, Dummy> withoutException = x =>
          TestDataGenerator = TestDataGeneratorFactory.Create (TestDataDomainConfiguration);
      ;

      return
          c =>
              c.Given ("using MaxRecursionDepth of " + recursionDepth, x => MaxRecursionDepth = recursionDepth)
                  .Given ("create test data generator", catchExceptions ? withExceptions : withoutException);
    }

    protected Context BaseDomainContext (bool useDefaults = true, int? seed = null)
    {
      return c => c
          .Given ((useDefaults ? "default domain" : "empty domain") + " with seed " + (!seed.HasValue ? "random" : seed.ToString ()),
              x =>
                  TestDataDomainConfiguration =
                      (configurator => configurator.UseDefaults (useDefaults).UseRandom (seed.HasValue ? new DefaultRandom (seed.Value) : DefaultRandom.Instance)))
          .Given (TestDataGeneratorContext ());
    }
  }
}