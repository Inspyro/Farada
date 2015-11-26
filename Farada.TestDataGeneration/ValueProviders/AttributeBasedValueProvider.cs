using System;
using System.Collections.Generic;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Provides a value for a member that has an attribute on it
  /// </summary>
  /// <typeparam name="TMember">The type of the member with the attribute</typeparam>
  /// <typeparam name="TAttribute">The type of the attribute</typeparam>
  /// <typeparam name="TContext">The type of the <see cref="IValueProviderContext"/></typeparam>
  public abstract class AttributeBasedValueProvider<TMember, TAttribute, TContext>:IValueProvider where TContext:AttributeValueProviderContext<TMember, TAttribute>
      where TAttribute : Attribute
  {
    protected abstract TMember CreateValue (TContext context);

    public object Create (IValueProviderContext context)
    {
      return CreateValue((TContext) context);
    }

    public IEnumerable<object> CreateMany (IValueProviderContext context, int numberOfObjects)
    {
      for (var i = 0; i < numberOfObjects; i++)
        yield return CreateValue ((TContext) context);
    }

    protected abstract TContext CreateContext (ValueProviderObjectContext objectContext);

    public bool CanHandle (Type memberType)
    {
      return memberType == typeof (TMember);
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
  /// <typeparam name="TMember">The type of the member</typeparam>
  /// <typeparam name="TAttribute">The type of the attribute</typeparam>
  public abstract class AttributeBasedValueProvider<TMember, TAttribute>:AttributeBasedValueProvider<TMember, TAttribute, AttributeValueProviderContext<TMember, TAttribute>>
      where TAttribute : Attribute
  {
    protected override AttributeValueProviderContext<TMember, TAttribute> CreateContext (ValueProviderObjectContext objectContext)
    {
      return new AttributeValueProviderContext<TMember, TAttribute>(objectContext);
    }
  }
  
}
