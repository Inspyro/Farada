using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Rubicon.RegisterNova.Infrastructure.TestData;
using Rubicon.RegisterNova.Infrastructure.TestData.RuleBasedDataGenerator;
using SpecK;
using SpecK.Specifications;

namespace Rubicon.RegisterNova.Infrastructure.UnitTests.TestData.IntegrationTest
{
  [SpecK.Subject (typeof (RuleBasedDataGenerator))]
  public class RuleBasedDataGeneratorSpeck : Specs
  {
    DomainConfiguration Domain;
    bool UseDefaults;
    RuleBasedDataGenerator DataGenerator;
    int MaxRecursionDepth;
    GeneratorResult InitialData;

    Context DefaultsContext (bool useDefaults)
    {
      return c => c.Given ("use defaults " + useDefaults, x => UseDefaults = useDefaults);
    }

    Context RecursionContext (int recursionDepth = 2)
    {
      return c => c.Given ("using MaxRecursionDepth of " + recursionDepth, x => MaxRecursionDepth = recursionDepth);
    }

    Context DataGeneratorContext ()
    {
      return
          c =>
              c.Given ("create rule based data generator",
                  x => DataGenerator = TestDataGeneratorFactory.CreateRuleBasedDataGenerator (Domain, UseDefaults));
    }

    Context BasePropertyContext (bool useDefaults = true, int recursionDepth = 2)
    {
      return c => c.Given (DefaultsContext (useDefaults))
          .Given (RecursionContext (recursionDepth))
          .Given (DataGeneratorContext ());
    }

    Context StringDomainContext (bool useDefaults = true)
    {
      return c => c
          .Given ("empty base domain", x => Domain = new DomainConfiguration
                                                     {
                                                         Rules = new RuleSet (new StringMarryRule ())
                                                     })
          .Given (BasePropertyContext (useDefaults))
          .Given(StringInitialDataContext());
    }

    Context StringInitialDataContext ()
    {
      return c => c.Given ("1000 sexy strings", x =>
      {
        var initialDataProvider = DataGenerator.InitialDataProvider;
        for (var i = 0; i < 1000; i++)
        {
          initialDataProvider.Add ("some sexy string " + i);
        }

        InitialData = initialDataProvider.Build ();
      });
    }

    [Group]
    void GetFactory ()
    {
      Specify (x => DataGenerator.Generate (1, InitialData))
          .Elaborate ("String Church", _ => _
              .Given (StringDomainContext ())
              .It ("successfully gets 50% of the strings married",
                  x => x.Result.GetResult<string> ().Count (resultString => resultString.Contains ("Marr")).Should ().Be (500)));
    }


    public class StringMarryRule : Rule
    {
      public override float GetExecutionProbability ()
      {
        return 0.5f;
      }

      protected override IEnumerable<IRuleParameter> GetRuleInputs ()
      {
        Func<RuleValue<string>, bool> predicate = p => p.Value.Length > 3 && (p.UserData.IsMarried == null || !p.UserData.IsMarried);
        yield return new RuleParameter<string> (predicate);
        yield return new RuleParameter<string> (predicate); //TODO: how to define excludes on rule filter basis?
      }

      protected override IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData) //e.g. one instance - stores all generation data..
      {
        var sexyString1 = inputData.GetValue<string> (0);
        var sexyString2 = inputData.GetValue<string> (1);

        sexyString1.Value += "[Married to:" + sexyString2.Value + "]";
        sexyString2.Value += "[Married to:" + sexyString1.Value + "]";

        sexyString1.UserData.IsMarried = true;
        sexyString2.UserData.IsMarried = true;

        yield break;
      }
    }
  }
}