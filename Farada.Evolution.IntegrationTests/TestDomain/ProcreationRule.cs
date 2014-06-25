using System;
using System.Collections.Generic;
using Farada.Evolution.RuleBasedDataGeneration;
using Farada.Evolution.Utilities;

namespace Farada.Evolution.IntegrationTests.TestDomain
{
  internal class ProcreationRule : Rule
  {
    public override float GetExecutionProbability ()
    {
      //TODO: Rule Appliance Probability based on World.Fertility...
      var fertility = World.Read<int?> (x => x.Fertility);
       
      return LerpUtility.LerpFromLowToHigh (100000, World.Count<Person> (), 1f, 0.1f);
    }

    protected override IEnumerable<IRuleParameter> GetRuleInputs ()
    {
      //TODO: can we have relations between persons? a likes b,c,..  d hates f,g..?
      yield return new RuleParameter<Person> ( p => p.Value.Age >= 14 && p.Value.Gender == Gender.Male);
      yield return new RuleParameter<Person> (p => p.Value.Age >= 14 && p.Value.Gender == Gender.Female && (p.UserData.IsPregnant == null || !p.UserData.IsPregnant));

      //TODO: do we need optional rule paramters?
    }

    protected override IEnumerable<IRuleValue> Execute (CompoundRuleInput inputData)
    {
      var male = inputData.GetValue<Person> (0);
      var female = inputData.GetValue<Person> (1);

      var childCount = 1; // TODO: Get Random object from somewhere ValueProvider.Random.Next(1, 1);
      for (var i = 0; i < childCount; i++)
      {
        var child = TestDataGenerator.Create<Person> ();
        child.Father = male.Value;
        child.Mother = female.Value;

        yield return new RuleValue<Person> (child);
      }

      female.UserData.IsPregnant = true;
    }
  }
}