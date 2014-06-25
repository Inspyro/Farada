using System;
using Farada.TestDataGeneration.CompoundValueProvider;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.ValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  public interface IValueProviderContext
  {
  }

  public class ValueProviderContext<TProperty>:IValueProviderContext
  {
    public Random Random { get; private set; }
    public Func<TProperty> GetPreviousValue { get; private set; }
    public Type PropertyType { get; private set; }
    public IFastPropertyInfo PropertyInfo { get; private set; }
    public ITestDataGenerator TestDataGenerator { get; private set; }

    protected internal ValueProviderContext (ValueProviderObjectContext objectContext)
    {
      TestDataGenerator = objectContext.TestDataGenerator;
      Random = objectContext.Random;
      GetPreviousValue = () => (TProperty) objectContext.GetPreviousValue();
      PropertyType = objectContext.PropertyType;
      PropertyInfo = objectContext.PropertyInfo;
    }
  }

  public class ValueProviderObjectContext
  {
    public Random Random { get; private set; }
    public Func<object> GetPreviousValue { get; private set; }
    public Type PropertyType { get; private set; }
    public IFastPropertyInfo PropertyInfo { get; private set; }
    public ITestDataGenerator TestDataGenerator { get; private set; }

    internal ValueProviderObjectContext (
        ITestDataGenerator testDataGenerator,
        Random random,
        Func<object> getPreviousValue,
        Type propertyType,
        IFastPropertyInfo fastPropertyInfo)
    {
      TestDataGenerator = testDataGenerator;
      Random = random;
      GetPreviousValue = getPreviousValue;
      PropertyType = propertyType;
      PropertyInfo = fastPropertyInfo;
    }
  }
}