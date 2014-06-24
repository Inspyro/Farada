using System;
using Farada.Core.ValueProvider;
using Rubicon.RegisterNova.Infrastructure.Utilities;

namespace Farada.Core.BaseDomain.ValueProviders
{
  internal class RandomFloatGenerator:ValueProvider<float>
  {
    protected override float CreateValue (ValueProviderContext<float> context)
    {
      return context.Random.Next(float.MinValue);
    }
  }
}