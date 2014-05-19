using System;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  public class RandomStringGenerator:ValueProvider<string>
  {
    protected override string CreateValue (ValueProviderContext<string> context)
    {
      return new string(Enumerable.Range(1, context.Random.Next(1,20)).Select(x => GenerateChar(context)).ToArray());
    }

    private char GenerateChar (ValueProviderContext<string> context)
    {
      return (char) context.Random.Next((byte) 'A', (byte) 'z' + 1);
    }
  }
}
