using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  internal class RandomFloatGenerator:ValueProvider<float>
  {
    protected override float CreateValue (ValueProviderContext<float> context)
    {
      return context.Random.Next(float.MinValue);
    }
  }
}