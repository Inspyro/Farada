using System;
using Farada.TestDataGeneration.ValueProviders;
using Farada.TestDataGeneration.ValueProviders.Context;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.BaseDomain.ValueProviders
{
  /// <summary>
  /// Creates a random enum for any enum type (<see cref="SubTypeValueProvider{TMember}"/>
  /// </summary>
  public class RandomEnumGenerator:SubTypeValueProvider<Enum>
  {
    [CanBeNull]
    protected override Enum CreateValue (ValueProviderContext<Enum> context)
    {
      var enumNames = Enum.GetNames(context.TargetValueType);
      if (enumNames.Length == 0)
        return null;

      var randomValue = enumNames[context.Random.Next(enumNames.Length)];

      return (Enum) Enum.Parse(context.TargetValueType, randomValue);
    }

    //we indicate that we fill the enums, but enums are not auto-fillable anyway as they are primitive types. 
    public override ValueFillMode FillMode
    {
      get { return ValueFillMode.All; }
    }
  }
}