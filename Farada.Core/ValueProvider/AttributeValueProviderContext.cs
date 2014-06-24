using System;

namespace Farada.Core.ValueProvider
{
  public class AttributeValueProviderContext<TProperty, TAttribute>:ValueProviderContext<TProperty> where TAttribute:Attribute
  {
    protected internal AttributeValueProviderContext (ValueProviderObjectContext objectContext)
        : base(objectContext)
    {
      Attribute = objectContext.PropertyInfo.GetCustomAttribute<TAttribute>();
    }

    public TAttribute Attribute { get; private set; }
  }
}