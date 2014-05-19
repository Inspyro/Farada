using System;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
{
  public class AttributeValueProviderContext<TProperty, TAttribute>:ValueProviderContext<TProperty> where TAttribute:Attribute
  {
    internal AttributeValueProviderContext (ICompoundValueProvider valueProvider, Random random, Func<TProperty> getPreviousValue, IFastPropertyInfo fastPropertyInfo, TAttribute attribute)
        : base(valueProvider, random, getPreviousValue, fastPropertyInfo)
    {
      Attribute = attribute;
    }

    public TAttribute Attribute { get; private set; }
  }
}