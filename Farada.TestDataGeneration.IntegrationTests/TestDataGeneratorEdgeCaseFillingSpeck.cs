using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), "Create_Collections")]
  public class TestDataGeneratorEdgeCaseFillingSpeck : TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorEdgeCaseFillingSpeck ()
    {
      Specify (x => TestDataGenerator.Create<ClassWithList> (MaxRecursionDepth, null))
          .Case ("Properties Are Initialized", _ => _
              .Given (ConfigurationContext (cfg =>
                  cfg.UseDefaults (false)
                      .For<ClassWithList> ().AddProvider (new DefaultInstanceValueProvider<ClassWithList> ())
                      .For<IList<int>> ()
                      //
                        .AddProvider (f => new List<int> { 0, 1, 2, 3 })
                        .DisableAutoFill ()))
              .It ("initialized first list correctly", x => x.Result.IntegerList.Should ().BeEquivalentTo (new[] { 0, 1, 2, 3 })));
    }

    Context ConfigurationContext (TestDataDomainConfiguration config)
    {
      return c => c
          .Given ("domain with valid configuration", x => { TestDataDomainConfiguration = config; })
          .Given (TestDataGeneratorContext ());
    }
  }

  public class ClassWithList
  {
    public IList<int> IntegerList { get; set; }
  }
}