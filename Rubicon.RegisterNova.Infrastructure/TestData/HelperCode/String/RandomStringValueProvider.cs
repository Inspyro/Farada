using System;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.String
{
  public class RandomStringValueProvider:ValueProvider<string, RandomStringGenerator>
  {
    public RandomStringValueProvider (ValueProvider<string> nextProvider=null)
        : base(nextProvider)
    {
    }

    protected override string GetValue (string currentValue)
    {
      return RandomGenerator.Next();
    }
  }

  public class RandomStringGenerator:RandomGenerator<string>
  {
    public override string Next ()
    {
      return new string(Enumerable.Range(1, Random.Next(1,20)).Select(x => GenerateChar()).ToArray());
    }

    private char GenerateChar ()
    {
      return (char) Random.Next((byte) 'A', (byte) 'z' + 1);
    }
  }
}
