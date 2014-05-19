using System;
using Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider;

namespace Rubicon.RegisterNova.Infrastructure.TestData.HelperCode.ValueProviders
{
  internal class RandomEnumGenerator:ValueProvider<Enum>
  {
    protected override Enum CreateValue (ValueProviderContext<Enum> context)
    {
      var enumNames = Enum.GetNames(context.PropertyInfo.GetType());
      var randomValue = enumNames[context.Random.Next(enumNames.Length+1)];

      return (Enum) Enum.Parse(context.PropertyInfo.GetType(), randomValue);
    }
  }
}