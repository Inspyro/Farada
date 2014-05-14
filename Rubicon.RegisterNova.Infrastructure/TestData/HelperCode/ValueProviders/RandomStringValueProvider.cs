using System;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  public class RandomStringGenerator:ValueProvider<string>
  {
    protected override string CreateValue ()
    {
      return new string(Enumerable.Range(1, Context.Random.Next(1,20)).Select(x => GenerateChar()).ToArray());
    }

    private char GenerateChar ()
    {
      return (char) Context.Random.Next((byte) 'A', (byte) 'z' + 1);
    }
  }
}
