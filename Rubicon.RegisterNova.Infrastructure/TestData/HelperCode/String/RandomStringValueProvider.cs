using System;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.String
{
  public class RandomStringGenerator:ValueProvider<string>
  {
    protected override string GetValue ()
    {
      return new string(Enumerable.Range(1, Random.Next(1,20)).Select(x => GenerateChar()).ToArray());
    }

    private char GenerateChar ()
    {
      return (char) Random.Next((byte) 'A', (byte) 'z' + 1);
    }
  }
}
