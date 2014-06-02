using System;
using System.Linq;
using FluentAssertions;
using Rubicon.RegisterNova.Infrastructure.TestData;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using SpecK;
using SpecK.Extensibility;
using SpecK.Specifications;
using SpecK.Specifications.InferredApi;

namespace Rubicon.RegisterNova.Infrastructure.UnitTests.TestData.IntegrationTest
{
  [Subject (typeof (ICompoundValueProvider))]
  public class TestDataIntegrationSpeck : Specs
  {
    BaseDomainConfiguration Domain;
    bool UseDefaults;
    ICompoundValueProvider ValueProvider;
    int MaxRecursionDepth;

    [Group]
    void ValueProviderWithDefaultDomain ()
    {
      Domain = new BaseDomainConfiguration ();
      UseDefaults = true;
      ValueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider (Domain, UseDefaults);

      MaxRecursionDepth = 2;

      GenericCase<int> ("simple int case", _ => _
          //
          .It ("should be", x => x.Result.Should ().Be (10)));

      GenericCase<string> ("simple string case", _ => _
          //
          .It ("should equal test", x => x.Result.Should ().Be ("test")));
    }

    public void GenericCase<T>(string caseDescription, Func<IAgainstOrArrangeOrAssert<DontCare, T>, IAssert<DontCare, T>> succession)
    {
      Specify (x => ValueProvider.Create<T> (MaxRecursionDepth, null)).Elaborate (caseDescription, succession);
    }
  }
}