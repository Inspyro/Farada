using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode
{
  class BasicStringGenerator:ValueProvider<string>
  {
    public BasicStringGenerator (ValueProvider<string> nextProvider=null)
        : base(nextProvider)
    {
    }

    protected override string GetValue (string currentValue)
    {
      return base.GetValue(currentValue + "some String...");
    }
  }
}