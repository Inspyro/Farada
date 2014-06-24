using System;
using Farada.Core.CompoundValueProvider;
using Farada.Core.FastReflection;

namespace Farada.Core.ValueProvider
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
    public ICompoundValueProvider ValueProvider { get; private set; }

    protected internal ValueProviderContext (ValueProviderObjectContext objectContext)
    {
      ValueProvider = objectContext.ValueProvider;
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
    public ICompoundValueProvider ValueProvider { get; private set; }

    internal ValueProviderObjectContext (
        ICompoundValueProvider valueProvider,
        Random random,
        Func<object> getPreviousValue,
        Type propertyType,
        IFastPropertyInfo fastPropertyInfo)
    {
      ValueProvider = valueProvider;
      Random = random;
      GetPreviousValue = getPreviousValue;
      PropertyType = propertyType;
      PropertyInfo = fastPropertyInfo;
    }
  }
}