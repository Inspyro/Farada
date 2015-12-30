using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator),"Create")]
  public class TestDataGeneratorReflectiveFillingSpeck : TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorReflectiveFillingSpeck ()
    {
      Specify (x =>
          //here we use the Type based approached (compatible with System.Reflection).
          TestDataGenerator.Create (typeof(Universe), MaxRecursionDepth, null)
          )
          .Case ("should fill normal property", _ => _
              .Given (SimpleStringContext ())
              .It ("fills properties via reflection",
                  //we have to cast the result, as it is reflection based...
                  x => ((Universe) x.Result).Galaxy1.StarSystem1.Planet1.President.Name.Should ().Be ("SomeString")));
    }

    Context SimpleStringContext ()
    {
      return c => c.Given ("simple string domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            //register the first provider via Type rather than generics.
            .For(typeof(object)).AddProvider (new DefaultInstanceValueProvider<object> ())

            //Note: Here we register a special func provider that derives from ReflectiveValueProvider
            //A reflective value provider considers the type given to its ctor (see first arg) rather than the return value (object)
            .For(typeof(string)).AddProvider (typeof(string), context => "SomeString");
      })
      .Given(TestDataGeneratorContext());
    }
  }
}