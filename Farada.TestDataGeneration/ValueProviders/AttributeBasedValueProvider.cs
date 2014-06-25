using System;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Provides a value for a property that has an attribute on it
  /// </summary>
  /// <typeparam name="TProperty">The type of the property with the attribute</typeparam>
  /// <typeparam name="TAttribute">The type of the attribute</typeparam>
  /// <typeparam name="TContext">The type of the <see cref="IValueProviderContext"/></typeparam>
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

  /// <summary>
  /// A value provider for an attribute like <see cref="AttributeBasedValueProvider{TProperty,TAttribute,TContext}"/>
  /// but with the default TContext which is <see cref="AttributeValueProviderContext{TProperty,TAttribute}"/>
  /// </summary>
  /// <typeparam name="TProperty">The type of the property</typeparam>
  /// <typeparam name="TAttribute">The type of the attribute</typeparam>
  public abstract class AttributeBasedValueProvider<TProperty, TAttribute>:AttributeBasedValueProvider<TProperty, TAttribute, AttributeValueProviderContext<TProperty, TAttribute>>
      where TAttribute : Attribute
  {
    protected override AttributeValueProviderContext<TProperty, TAttribute> CreateContext (ValueProviderObjectContext objectContext)
    {
      return new AttributeValueProviderContext<TProperty, TAttribute>(objectContext);
    }
  }
  
}
