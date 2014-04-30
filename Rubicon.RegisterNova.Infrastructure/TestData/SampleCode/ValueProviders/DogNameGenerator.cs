using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueGeneration;

namespace Rubicon.RegisterNova.Infrastructure.TestData.SampleCode.ValueProviders
{
  internal class DogNameGenerator:ValueProvider<string>
  {
    private readonly string _additionalContent;

    public DogNameGenerator (string additionalContent, ValueProvider<string> nextProvider=null)
        : base(nextProvider)
    {
      _additionalContent = additionalContent;
    }

    protected override string GetValue (string currentValue)
    {
      return "I am a dog - " + _additionalContent;
    }
  }
}