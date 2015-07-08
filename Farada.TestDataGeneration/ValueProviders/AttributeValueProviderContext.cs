using System;
using System.Diagnostics;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// The default context for attribute value providers
  /// </summary>
  /// <typeparam name="TProperty">The type of the property</typeparam>
  /// <typeparam name="TAttribute">The type of the attribute</typeparam>
  public class AttributeValueProviderContext<TProperty, TAttribute>:ValueProviderContext<TProperty> where TAttribute:Attribute
  {
    protected internal AttributeValueProviderContext (ValueProviderObjectContext objectContext)
        : base(objectContext)
    {
      //REVIEW RN-242
      Trace.Assert (objectContext.MemberInfo != null);
      Attribute = objectContext.MemberInfo.GetCustomAttribute<TAttribute>();
    }

    /// <summary>
    /// The concrete attribute that is on the property
    /// </summary>
    public TAttribute Attribute { get; private set; }
  }
}