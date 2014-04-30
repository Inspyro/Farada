using System;
using Rubicon.RegisterNova.Infrastructure.TestData.SampleCode.Classes;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.SampleCode.ValueProviders
{
  internal class CatGenerator:ValueProvider<Cat>
  {
    public CatGenerator (ValueProvider<Cat> nextProvider=null)
        : base(nextProvider)
    {
    }

    protected override Cat GetValue (Cat currentValue)
    {
      return new Cat { Name = "Nice cat" };
    }
  }
}