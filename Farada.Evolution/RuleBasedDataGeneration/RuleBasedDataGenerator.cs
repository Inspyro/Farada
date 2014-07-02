using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProvider;
using Farada.TestDataGeneration.Extensions;
using Remotion.Utilities;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public class RuleBasedDataGenerator
  {
    private readonly Random _random;
    private readonly RuleSet _ruleSet;
    public ICompoundValueProvider ValueProvider { get; private set; }

    internal RuleBasedDataGenerator (ICompoundValueProvider compoundValueProvider, Random random, RuleSet ruleSet)
    {
      ArgumentUtility.CheckNotNull("compoundValueProvider", compoundValueProvider);

      ValueProvider = compoundValueProvider;
      _random = random;
      _ruleSet = ruleSet;
      InitialDataProvider = new InitialDataProvider(new GeneratorDataProvider(_random));
    }

    public GeneratorResult Generate (int generations = 1, GeneratorResult initialData = null)
    {
      if(_ruleSet==null)
      {
        throw new InvalidOperationException("You cannot generate data without a rule set - please specify a rule set on initiliation!");
      }

      var world = new World();

      var result = initialData;
      for (var i = 0; i < generations; i++)
      {
        result = Generate(result, world);
      }

      return result;
    }

    
    private GeneratorResult Generate (GeneratorResult lastGenerationResult, IWriteableWorld world)
    {
      var dataProvider = lastGenerationResult==null?new GeneratorDataProvider(_random):new GeneratorDataProvider(_random, lastGenerationResult.DataLists);
      var generatedData = new List<IRuleValue>();

      foreach (var rule in _ruleSet.GetRules())
      {
        var inputParameters = rule.GetRuleInputs(world);
        var inputDataList = inputParameters.Select(p => dataProvider.GetAll(p).ToList()).ToList();

        if(inputDataList.Count<=0)
          continue;

        var executionCount = (int) (rule.GetExecutionProbability() * inputDataList[0].Count);

        var inputList = new List<CompoundRuleInput>();
        foreach (var t in inputDataList)
        {
          var parameterValues = t;
          if (parameterValues.Count > executionCount)
          {
            parameterValues = parameterValues.Take(executionCount).ToList();
          }

          parameterValues.Randomize(_random);

          for (var i = 0; i < parameterValues.Count; i++)
          {
            var value = parameterValues[i];

            if (i >= inputList.Count)
            {
              inputList.Add(new CompoundRuleInput());
            }

            inputList[i].Add(value);
          }
        } 

        //TODO: Use plinq here... but never give them the same value - should be already like this
        foreach (var inputValues in inputList.Where(ruleInput=>ruleInput.Count==inputDataList.Count))
        {
          generatedData.AddRange(rule.Execute(inputValues, ValueProvider, world));
        }
      }

      foreach (var result in generatedData)
      {
        dataProvider.Add(result);
      }

      foreach (var globalRule in _ruleSet.GetGlobalRules())
      {
        globalRule.Execute(world);
      }

      return new GeneratorResult(dataProvider.DataLists);
    }

    public InitialDataProvider InitialDataProvider { get; private set; }
  }
}