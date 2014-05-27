using System;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
{
  //TODO: introducte TContext as in ValueProvider
  //TODO: Rename (E.g. AttributeBasedValueProvider)
  public abstract class AttributeValueProvider<TProperty, TAttribute>:IValueProvider where TAttribute:Attribute
  {
    protected abstract TProperty CreateValue (AttributeValueProviderContext<TProperty, TAttribute> context);

    public object CreateValue (IValueProviderContext context)
    {
      return CreateValue((AttributeValueProviderContext<TProperty, TAttribute>) context);
    }

    public IValueProviderContext CreateContext (
        ICompoundValueProvider compoundValueProvider,
        Random random,
        Func<object> getPreviousValue,
        Type propertyType,
        IFastPropertyInfo fastPropertyInfo)
    {
      return new AttributeValueProviderContext<TProperty, TAttribute>(
          compoundValueProvider,
          random,
          () => (TProperty) getPreviousValue(),
          propertyType,
          fastPropertyInfo,
          fastPropertyInfo.GetCustomAttribute<TAttribute>());
    }
  }
}
