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

        foreach (var parameterValues in inputDataList)
        {
          parameterValues.Randomize(_random);
        }

        var count = inputDataList.Select(inputData => inputData.Count).Concat(new[] { int.MaxValue }).Min();
        var executionCount = (int) (rule.GetExecutionProbability(world) * count);

        var inputList = new List<CompoundRuleInput>();
        for (var i = 0; i < count; i++)
        {
          var compoundRuleInput = new CompoundRuleInput();
          inputDataList.ForEach(values => compoundRuleInput.AddParameterValue(values[i]));

          if (compoundRuleInput.IsValidFor(inputParameters))
          {
            inputList.Add(compoundRuleInput);
          }

          if(inputList.Count>=executionCount)
            break;
        }

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