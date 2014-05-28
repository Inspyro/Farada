using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
{
  //TODO: introducte TContext as in ValueProvider
  public abstract class AttributeBasedValueProvider<TProperty, TAttribute>:IValueProvider where TAttribute:Attribute
  {
    protected abstract TProperty CreateValue (AttributeValueProviderContext<TProperty, TAttribute> context);

    public object CreateValue (IValueProviderContext context)
    {
      return CreateValue((AttributeValueProviderContext<TProperty, TAttribute>) context);
    }

    public bool CanHandle (Type propertyType)
    {
      return propertyType == typeof (TProperty);
    }

    public IValueProviderContext CreateContext (ValueProviderObjectContext objectContext)
    {
      return new AttributeValueProviderContext<TProperty, TAttribute>(objectContext, objectContext.PropertyInfo.GetCustomAttribute<TAttribute>());
    }
  }
}
