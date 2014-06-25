using System;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  internal class RandomFloatGenerator:ValueProvider<float>
  {
    protected override float CreateValue (ValueProviderContext<float> context)
    {
      return context.Random.Next(float.MinValue);
    }
  }
}