using System;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
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
    public IFastPropertyInfo PropertyInfo { get; private set; }
    public ICompoundValueProvider ValueProvider { get; private set; }

    internal ValueProviderContext (
        ICompoundValueProvider valueProvider,
        Random random,
        Func<TProperty> getPreviousValue,
        IFastPropertyInfo fastPropertyInfo)
    {
      ValueProvider = valueProvider;
      Random = random;
      GetPreviousValue = getPreviousValue;
      PropertyInfo = fastPropertyInfo;
    }
  }
}