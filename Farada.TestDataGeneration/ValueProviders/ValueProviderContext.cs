using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// The concrete value provider context
  /// </summary>
  public interface IValueProviderContext
  {
  }

  /// <summary>
  /// The type safe value provider context used to create values in a <see cref="IValueProvider"/>
  /// </summary>
  /// <typeparam name="TProperty"></typeparam>
  public class ValueProviderContext<TProperty>:IValueProviderContext
  {
    /// <summary>
    /// The random to use for random value generation
    /// </summary>
    public Random Random { get; private set; }

    /// <summary>
    /// The func that referes to the previous value generator in the chain
    /// </summary>
    public Func<TProperty> GetPreviousValue { get; private set; } //TODO: Use method

    /// <summary>
    /// The type of the value to generate
    /// </summary>
    public Type TargetValueType { get; private set; }

    /// <summary>
    /// The property for which the value will be generated - can be null
    /// </summary>
    public IFastPropertyInfo PropertyInfo { get; private set; }

    /// <summary>
    /// The TestDataGenerator that can be used to generate values of other types (Note: avoid circular dependencies)
    /// </summary>
    public ITestDataGenerator TestDataGenerator { get; private set; }

    protected internal ValueProviderContext (ValueProviderObjectContext objectContext)
    {
      TestDataGenerator = objectContext.TestDataGenerator;
      Random = objectContext.Random;
      GetPreviousValue = () => (TProperty) objectContext.GetPreviousValue();
      TargetValueType = objectContext.TargetValueType;
      PropertyInfo = objectContext.PropertyInfo;
    }
  }
}