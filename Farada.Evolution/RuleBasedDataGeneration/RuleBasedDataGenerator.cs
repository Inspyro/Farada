using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;
using Remotion.Utilities;

namespace Farada.Evolution.RuleBasedDataGeneration
{
  public class RuleBasedDataGenerator
  {
    private readonly IRandom _random;
    private readonly RuleSet _ruleSet;
    public ITestDataGenerator TestDataGenerator { get; }

    internal RuleBasedDataGenerator (ITestDataGenerator testDataGenerator, RuleSet ruleSet)
    {
      ArgumentUtility.CheckNotNull("testDataGenerator", testDataGenerator);

      TestDataGenerator = testDataGenerator;
      _random = testDataGenerator.Random;
      _ruleSet = ruleSet;
      InitialDataProvider = new InitialDataProvider(new GeneratorDataProvider());
    }

    public GeneratorResult Generate (int generations = 1, GeneratorResult initialData = null)
    {
      if(_ruleSet==null)
      {
        throw new InvalidOperationException("You cannot generate data without a rule set - please specify a rule set on initiliation!");
      }

      var world = new World(initialData);
      for (var i = 0; i < generations; i++)
      {
        Generate(world);
      }

      return world.CurrentData;
    }

    
    private void Generate (World world)
    {
      var dataProvider = world.CurrentData==null?new GeneratorDataProvider():new GeneratorDataProvider(world.CurrentData.DataLists);

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

        var count = inputDataList.Any(inputData => inputData.Count <= 0) ? 0 : inputDataList.Select(inputData => inputData.Count).Max();
        
        var executionCount = (int) Math.Round(rule.GetExecutionProbability(world) * count);

        var inputList = new List<CompoundRuleInput>();
        for (var i = 0; i < count; i++)
        {
          if(inputList.Count>=executionCount)
            break;

          var compoundRuleInput = new CompoundRuleInput();
          inputDataList.ForEach(values => compoundRuleInput.AddParameterValue(values[i%values.Count]));

          if (compoundRuleInput.IsValidFor(inputParameters))
          {
            inputList.Add(compoundRuleInput);
          }
        }

        //TODO: Use plinq here... but never give them the same value
        foreach (var inputValues in inputList.Where(ruleInput=>ruleInput.Count==inputDataList.Count))
        {
          generatedData.AddRange(rule.Execute(new CompoundRuleExecutionContext(inputValues, TestDataGenerator, world)));
        }
      }

      foreach (var result in generatedData)
      {
        dataProvider.Add(result);
      }

      world.CurrentData = new GeneratorResult(dataProvider.DataLists);
    }

    public InitialDataProvider InitialDataProvider { get; private set; }
  }
}