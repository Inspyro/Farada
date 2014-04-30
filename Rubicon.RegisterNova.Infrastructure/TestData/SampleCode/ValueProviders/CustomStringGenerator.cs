using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.SampleCode.ValueProviders
{
  internal class CustomStringGenerator : ValueProvider<string>
  {
    public CustomStringGenerator (ValueProvider<string> nextProvider=null)
        : base(nextProvider)
    {
    }

    protected override string GetValue (string currentValue)
    {
      return base.GetValue(currentValue.Substring(1,2));
    }
  }
}