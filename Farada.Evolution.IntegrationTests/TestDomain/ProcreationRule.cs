using System;
using System.Collections.Generic;
using Farada.Evolution.RuleBasedDataGeneration;

namespace Farada.Evolution.IntegrationTests.TestDomain
{
  internal class ProcreationRule : Rule
  {
    public override float GetExecutionProbability (IReadableWorld world)
    {
      var fertility = world.Read<float?> (x => x.Fertility);

      if (!fertility.HasValue)
        throw new ArgumentException ("You need a global rule that declares the world fertility");

      return fertility.Value;
    }

    public override IEnumerable<IRuleParameter> GetRuleInputs (IReadableWorld world)
    {
      yield return new RuleParameter<Person> (p => p.Value.Age >= 14 && p.Value.Gender == Gender.Male,
          (p, ruleInput) => p.Value.Likes (ruleInput.GetValue<Person> (1).Value) ? null : new[] { 1 });

      yield return
          new RuleParameter<Person> (
              p => p.Value.Age >= 14 && p.Value.Gender == Gender.Female && (p.UserData.IsPregnant == null || !p.UserData.IsPregnant),
              (p, ruleInput) => p.Value.Likes (ruleInput.GetValue<Person> (0).Value) ? null : new[] { 0 });
    }

    public override IEnumerable<IRuleValue> Execute (CompoundRuleExecutionContext context)
    {
      var male = context.InputData.GetValue<Person> (0);
      var female = context.InputData.GetValue<Person> (1);

      var childCount = context.TestDataGenerator.Random.Next(1,1); 
      for (var i = 0; i < childCount; i++)
      {
        var child = context.TestDataGenerator.Create<Person> ();
        child.Father = male.Value;
        child.Mother = female.Value;

        yield return new RuleValue<Person> (child);
      }

      female.UserData.IsPregnant = true;
    }
  }
}