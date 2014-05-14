using System;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
{
  /// <summary>
  /// TODO
  /// </summary>
  public class ValueProviderContext // TODO: Make generic.
  {
    public Random Random { get; private set; }
    public Func<object> GetPreviousValue { get; private set; }
    public IFastPropertyInfo PropertyInfo { get; private set; }
    public ICompoundValueProvider ValueProvider { get; private set; }

    internal ValueProviderContext(ICompoundValueProvider compoundValueProvider, Random random, IFastPropertyInfo propertyInfo, Func<object> getPreviousValueFunc)
    {
      ValueProvider = compoundValueProvider;
      Random = random;
      PropertyInfo = propertyInfo;
      GetPreviousValue = getPreviousValueFunc;
    }
  }
}