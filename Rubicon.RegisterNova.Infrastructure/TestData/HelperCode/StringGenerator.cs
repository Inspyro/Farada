using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode
{
  public class StringGenerator:RandomGenerator<string>
  {
    public override string Next ()
    {
      return "Some random string...";
    }
  }
}