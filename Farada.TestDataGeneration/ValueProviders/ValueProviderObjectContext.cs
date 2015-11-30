using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// The non type safe context which contains all values used in the type safe context <see cref="ValueProviderContext{TMember}"/>
  /// </summary>
  public class ValueProviderObjectContext
  {
    private AdvancedContext _advanced;
    internal IRandom Random { get; private set; }
    internal Func<DependedPropertyCollection, object> GetPreviousValue { get; private set; }
    internal Type TargetValueType { get; private set; }

    [CanBeNull]
    public IFastMemberWithValues Member { get; private set; }

    internal ITestDataGenerator TestDataGenerator { get; private set; }

    public AdvancedContext Advanced { get; private set; }


    protected internal ValueProviderObjectContext (
        ITestDataGenerator testDataGenerator,
        Func<DependedPropertyCollection, object> getPreviousValue,
        Type targetValueType,
        AdvancedContext advanced,
        [CanBeNull] IFastMemberWithValues member)
    {
      TestDataGenerator = testDataGenerator;
      Random = testDataGenerator.Random;
      GetPreviousValue = getPreviousValue;
      TargetValueType = targetValueType;
      Advanced = advanced;
      Member = member;
    }

    public class AdvancedContext
    {
      /// <summary>
      /// The key that can be used to create objects.
      /// </summary>
      public IKey Key { get; private set; }

      /// <summary>
      /// The dependencies for each property.
      /// </summary>
      public Dictionary<IKey, IList<IKey>> DependencyMapping { get; private set; }

      /// <summary>
      /// The conversion service between ctor params and properties.
      /// </summary>
      public IParameterConversionService ParameterConversionService { get; private set; }

      public ITestDataGeneratorAdvanced AdvancedTestDataGenerator { get; private set; }

      public AdvancedContext (IKey key, Dictionary<IKey, IList<IKey>> dependencyMapping,  IParameterConversionService parameterConversionService, ITestDataGeneratorAdvanced advancedTestDataGenerator)
      {
        Key = key;
        DependencyMapping = dependencyMapping;
        ParameterConversionService = parameterConversionService;
        AdvancedTestDataGenerator = advancedTestDataGenerator;
      }
    }
  }
}