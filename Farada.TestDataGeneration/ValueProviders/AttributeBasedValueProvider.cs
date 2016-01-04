using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Farada.TestDataGeneration.ValueProviders.Context;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Provides a value for a member that has an attribute on it
  /// </summary>
  /// <typeparam name="TMember">The type of the member with the attribute</typeparam>
  /// <typeparam name="TAttribute">The type of the attribute</typeparam>
  //TODO: Create another context for single attribute. Also add validation for "AllowMultiple".
  //TODO: Create test for multiple attributes (AllowMultiple).
  public abstract partial class AttributeBasedValueProvider<TMember, TAttribute>
      : ValueProvider<TMember, AttributeValueProviderContext<TMember, TAttribute>>
      where TAttribute : Attribute
  {

    protected override sealed TMember CreateValue (AttributeValueProviderContext<TMember, TAttribute> context)
    {
      if (context.Attributes.Count == 0) //no attributes found - go to previous provider.
        return context.GetPreviousValue();

      Debug.Assert (context.Attribute != null, "The attribute was null, but should never be null");
      return CreateAttributeBasedValue (context);
    }

    protected abstract TMember CreateAttributeBasedValue (AttributeValueProviderContext<TMember, TAttribute> context);

    protected override AttributeValueProviderContext<TMember, TAttribute> CreateContext (ValueProviderObjectContext objectContext)
    {
      var customAttributes = objectContext.Member?.GetCustomAttributes<TAttribute>().ToList() ?? new List<TAttribute>();
      return new AttributeValueProviderContext<TMember, TAttribute> (objectContext, customAttributes);
    }
  }

  public class AttributeValueProviderContext<TMember, TAttribute> : ValueProviderContext<TMember>
  {
    public TAttribute Attribute { get; }
    public IList<TAttribute> Attributes { get; }

    public AttributeValueProviderContext([NotNull] ValueProviderObjectContext objectContext, IList<TAttribute> attributes)
        : base(objectContext)
    {
      Attribute = attributes.Count > 0 ? attributes[0] : default(TAttribute);
      Attributes = attributes;
    }
  }
}