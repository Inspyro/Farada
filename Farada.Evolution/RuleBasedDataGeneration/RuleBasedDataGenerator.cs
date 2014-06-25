using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Extensions;
using Remotion.Utilities;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public class RuleBasedDataGenerator
  {
    private readonly Random _random;
    private readonly RuleSet _ruleSet;
    public ITestDataGenerator TestDataGenerator { get; private set; }

    internal RuleBasedDataGenerator (ITestDataGenerator testDataGenerator, RuleSet ruleSet)
    {
      ArgumentUtility.CheckNotNull("testDataGenerator", testDataGenerator);

      TestDataGenerator = testDataGenerator;
      _random = testDataGenerator.Random;
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

      foreach (var globalRule in _ruleSet.GetGlobalRules())
      {
        globalRule.Execute(world);
      }

      foreach (var rule in _ruleSet.GetRules())
      {
        var inputParameters = rule.GetRuleInputs(world).ToList();
        var inputDataList = inputParameters.Select(p => dataProvider.GetAll(p).ToList()).ToList();

        if (inputDataList.Count <= 0)
          continue;

        var executionCount = (int) (rule.GetExecutionProbability(world) * inputDataList[0].Count);

        var inputList = new List<CompoundRuleInput>();
        foreach (var parameterValues in inputDataList)
        {
          var currentParameterValues = parameterValues;
          currentParameterValues.Randomize(_random);

          for (var i = 0; i < currentParameterValues.Count; i++)
          {
            var value = currentParameterValues[i];

            if (i >= inputList.Count)
            {
              inputList.Add(new CompoundRuleInput());
            }

            inputList[i].Add(value);
          }
        }

        //TODO: unfinished code...
        var nextIndex = 1;
        for (int i = 0; i < inputList.Count-1; i=nextIndex)
        {
          var compoundRuleInput = inputList[i];

          var x = 0;
          foreach (IRuleValue input in compoundRuleInput)
          {
            var parameter = inputParameters[x];
            var notLikableParameters=parameter.ParameterPredicate(input, compoundRuleInput);

            x++;

            if(notLikableParameters==null)
              continue;

            foreach (var notLikableParameter in notLikableParameters)
            {
              compoundRuleInput[notLikableParameter] = inputList[nextIndex][notLikableParameter];
            }

            nextIndex++;
            i--;
          }

          nextIndex++;
        }

        foreach (var compoundRuleInput in inputList.Where(compoundRuleInput => compoundRuleInput.Count > executionCount))
        {
          compoundRuleInput.ShrinkTo(executionCount);
        }

        //TODO: The input list is already filtered by the execution count...

        //TODO: Use plinq here... but never give them the same value - should be already like this
        foreach (var inputValues in inputList.Where(ruleInput=>ruleInput.Count==inputDataList.Count))
        {
          generatedData.AddRange(rule.Execute(new CompoundRuleExecutionContext(inputValues, TestDataGenerator, world)));
        }
      }

      foreach (var result in generatedData)
      {
        dataProvider.Add(result);
      }

      return new GeneratorResult(dataProvider.DataLists);
    }

    public InitialDataProvider InitialDataProvider { get; private set; }
  }
}