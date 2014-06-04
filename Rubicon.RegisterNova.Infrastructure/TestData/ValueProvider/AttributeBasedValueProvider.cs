using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueProvider
{
  //TODO: introducte TContext as in ValueProvider
  public abstract class AttributeBasedValueProvider<TProperty, TAttribute, TContext>:IValueProvider where TContext:IValueProviderContext
      where TAttribute : Attribute
  {
    protected abstract TProperty CreateValue (TContext context);

    public object CreateValue (IValueProviderContext context)
    {
      return CreateValue((TContext) context);
    }

    protected abstract TContext CreateContext (ValueProviderObjectContext objectContext);

    public bool CanHandle (Type propertyType)
    {
      return propertyType == typeof (TProperty);
    }

    IValueProviderContext IValueProvider.CreateContext (ValueProviderObjectContext objectContext)
    {
      return CreateContext(objectContext);
    }
  }

  public abstract class AttributeBasedValueProvider<TProperty, TAttribute>:AttributeBasedValueProvider<TProperty, TAttribute, AttributeValueProviderContext<TProperty, TAttribute>>
      where TAttribute : Attribute
  {
    protected override AttributeValueProviderContext<TProperty, TAttribute> CreateContext (ValueProviderObjectContext objectContext)
    {
      return new AttributeValueProviderContext<TProperty, TAttribute>(objectContext);
    }
  }
  
}
