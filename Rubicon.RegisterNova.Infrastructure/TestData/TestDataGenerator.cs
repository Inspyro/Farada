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
    public TypeValueProvider ValueProvider { get; private set; }

    internal TestDataGenerator (TypeValueProvider valueProvider)
    {
      ValueProvider = valueProvider;
      InitialDataProvider = new InitialDataProvider(new GeneratorDataProvider(ValueProvider.Random));
    }

    public GeneratorResult Generate (RuleSet ruleSet, GeneratorResult lastGenerationResult=null)
    {
      ArgumentUtility.CheckNotNull("ruleSet", ruleSet);

      var dataProvider = lastGenerationResult==null?new GeneratorDataProvider(ValueProvider.Random):new GeneratorDataProvider(ValueProvider.Random, lastGenerationResult.DataLists);

      foreach (var ruleEvent in ruleSet.GetRuleAppliances(dataProvider.GetCountInternal))
      {
        var inputParameters = ruleEvent.Rule.GetRuleInputs();
        var inputDataList = inputParameters.Select(p => dataProvider.GetAll(p).ToArray()).ToList();

        var inputList = new List<List<IRuleInput>>();
        foreach (var t in inputDataList)
        {
          var parameterData = t;
          if (parameterData.Length > ruleEvent.ExecutionCount)
          {
            parameterData = parameterData.Take(ruleEvent.ExecutionCount).ToArray();
          }

          parameterData.Randomize(ValueProvider.Random);


          for (var i = 0; i < parameterData.Length; i++)
          {
            var parameter = parameterData[i];

            if (i >= inputList.Count)
            {
              inputList.Add(new List<IRuleInput> { parameter });
            }
            else
            {
              inputList[i].Add(parameter);
            }
          }
        }


        foreach (var inputValues in inputList.Where(parameterList=>parameterList.Count==inputDataList.Count))
        {
          ruleEvent.Rule.Execute(inputValues, dataProvider, ValueProvider);
        }
      }

      foreach (var globalRule in ruleSet.GetGlobalRules())
      {
        var handleList=dataProvider.GetHandleListInternal(globalRule.MainDataType);
        foreach (var handle in handleList)
        {
          globalRule.Execute(handle);
        }
      }

      return new GeneratorResult(dataProvider.DataLists);
    }

    public InitialDataProvider InitialDataProvider { get; private set; }
  }

  public class InitialDataProvider
  {
    private readonly GeneratorDataProvider _generatorDataProvider;

    public InitialDataProvider (GeneratorDataProvider generatorDataProvider)
    {
      _generatorDataProvider = generatorDataProvider;
    }

    public void Add<T>(T value)
    {
      _generatorDataProvider.Add(value);
    }

    public GeneratorResult Build()
    {
      return new GeneratorResult(_generatorDataProvider.DataLists);
    }
  }
}