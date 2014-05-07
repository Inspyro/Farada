using System;
using System.Collections.Generic;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.DataGeneration;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueChain;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData
{
  public class TestDataGenerator
  {
    private readonly RuleSet _ruleSet;
    public CompoundValueProvider ValueProvider { get; private set; }

    internal TestDataGenerator (CompoundValueProvider valueProvider, RuleSet ruleSet)
    {
      ArgumentUtility.CheckNotNull("valueProvider", valueProvider);

      ValueProvider = valueProvider;
      _ruleSet = ruleSet;
      InitialDataProvider = new InitialDataProvider(new GeneratorDataProvider(ValueProvider.Random));
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
      var dataProvider = lastGenerationResult==null?new GeneratorDataProvider(ValueProvider.Random):new GeneratorDataProvider(ValueProvider.Random, lastGenerationResult.DataLists);
      var generatedData = new List<IRuleValue>();

      foreach (var rule in _ruleSet.GetRules())
      {
        var inputParameters = rule.GetRuleInputs(world);
        var inputDataList = inputParameters.Select(p => dataProvider.GetAll(p).ToArray()).ToList();

        var executionCount = (int) (rule.GetExecutionProbability() * inputDataList.Count); //TODO count to low

        var inputList = new List<CompoundRuleInput>();
        foreach (var t in inputDataList)
        {
          var parameterValues = t;
          if (parameterValues.Length > executionCount)
          {
            parameterValues = parameterValues.Take(executionCount).ToArray();
          }

          parameterValues.Randomize(ValueProvider.Random);

          for (var i = 0; i < parameterValues.Length; i++)
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