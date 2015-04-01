using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// The non type safe context which contains all values used in the type safe context <see cref="ValueProviderContext{TProperty}"/>
  /// </summary>
  public class ValueProviderObjectContext
  {
    internal Random Random { get; private set; }
    internal Func<object> GetPreviousValue { get; private set; }
    internal Type TargetValueType { get; private set; }

    [CanBeNull]
    internal IFastPropertyInfo PropertyInfo { get; private set; }

    internal ITestDataGenerator TestDataGenerator { get; private set; }

    protected internal ValueProviderObjectContext (
        ITestDataGenerator testDataGenerator,
        Func<object> getPreviousValue,
        Type targetValueType,
        [CanBeNull] IFastPropertyInfo fastPropertyInfo)
    {
      TestDataGenerator = testDataGenerator;
      Random = testDataGenerator.Random;
      GetPreviousValue = getPreviousValue;
      TargetValueType = targetValueType;
      PropertyInfo = fastPropertyInfo;
    }
  }
}