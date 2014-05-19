using System;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
{
  public abstract class AttributeValueProvider<TProperty, TAttribute>:IValueProvider where TAttribute:Attribute
  {
    protected abstract TProperty CreateValue (AttributeValueProviderContext<TProperty, TAttribute> context);

    public object CreateObjectValue (IValueProviderContext context)
    {
      return CreateValue((AttributeValueProviderContext<TProperty, TAttribute>) context);
    }

    public IValueProviderContext CreateContext (
        ICompoundValueProvider compoundValueProvider,
        Random random,
        Func<object> getPreviousValue,
        IFastPropertyInfo fastPropertyInfo)
    {
      return new AttributeValueProviderContext<TProperty, TAttribute>(
          compoundValueProvider,
          random,
          () => (TProperty) getPreviousValue(),
          fastPropertyInfo,
          fastPropertyInfo.GetCustomAttribute<TAttribute>());
    }
  }
}
