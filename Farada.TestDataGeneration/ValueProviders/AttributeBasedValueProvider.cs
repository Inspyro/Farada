using System;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Provides a value for a member that has an attribute on it
  /// </summary>
  /// <typeparam name="TMember">The type of the member with the attribute</typeparam>
  /// <typeparam name="TAttribute">The type of the attribute</typeparam>
  public abstract class AttributeBasedValueProvider<TMember, TAttribute>
      : ExtendedValueProvider<TMember, TAttribute>
      where TAttribute : Attribute
  {
    protected override TAttribute CreateData (ValueProviderObjectContext objectContext)
    {
      if (objectContext.Member == null)
      {
        throw new ArgumentException (
            $"Expected objectContext.Member != null for {nameof (AttributeBasedValueProvider<TMember, TAttribute>)} but got null member.");
      }

      var customAttribute = objectContext.Member.GetCustomAttribute<TAttribute>();

      if (customAttribute == null)
      {
        throw new ArgumentException (
            $"Expected member {objectContext.Member.Name} to have an attribute of type {typeof (TAttribute).FullName} but did not find such an attribute. {nameof (AttributeBasedValueProvider<TMember, TAttribute>)}");
      }

      return customAttribute;
    }
  }
}