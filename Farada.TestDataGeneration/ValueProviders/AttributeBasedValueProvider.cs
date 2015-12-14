using System;
using System.Collections.Generic;
using System.Linq;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Provides a value for a member that has an attribute on it
  /// </summary>
  /// <typeparam name="TContainer">The container of the member</typeparam>
  /// <typeparam name="TMember">The type of the member with the attribute</typeparam>
  /// <typeparam name="TAttribute">The type of the attribute</typeparam>
  public abstract class AttributeBasedValueProvider<TContainer, TMember, TAttribute>
      : ExtendedValueProvider<TContainer, TMember, IList<TAttribute>>
      where TAttribute : Attribute
  {
    protected sealed override TMember CreateValue (ExtendedValueProviderContext<TContainer, TMember, IList<TAttribute>> context)
    {
      if(context.AdditionalData.Count==0) //no attributes found - go to previous provider.
        return context.GetPreviousValue(); //TODO: Throw exception if no previous value exists.

      return CreateAttributeBasedValue(context);
    }

    protected abstract TMember CreateAttributeBasedValue (ExtendedValueProviderContext<TContainer, TMember, IList<TAttribute>> context);

    protected override IList<TAttribute> CreateData (ValueProviderObjectContext objectContext)
    {
      return objectContext.Member?.GetCustomAttributes<TAttribute>().ToList() ?? new List<TAttribute>();
    }
  }

  //TODO: Create attribute context because of api usage (AdditionalData is not the perfect name).
  //TODO: Create another context for single attribute. Also add validation for "AllowMultiple".
  //TODO: Create test for multiple attributes (AllowMultiple).
  public abstract class AttributeBasedValueProvider<TMember, TAttribute>
     : ExtendedValueProvider<TMember, IList<TAttribute>>
     where TAttribute : Attribute
  {
    protected sealed override TMember CreateValue(ExtendedValueProviderContext<TMember, IList<TAttribute>> context)
    {
      if (context.AdditionalData.Count == 0) //no attributes found - go to previous provider.
        return context.GetPreviousValue(); //TODO: Throw exception if no previous value exists.

      return CreateAttributeBasedValue(context);
    }

    protected abstract TMember CreateAttributeBasedValue(ExtendedValueProviderContext<TMember, IList<TAttribute>> context);

    protected override IList<TAttribute> CreateData(ValueProviderObjectContext objectContext)
    {
      return objectContext.Member?.GetCustomAttributes<TAttribute>().ToList() ?? new List<TAttribute>();
    }
  }
}