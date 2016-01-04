using System;
using Farada.TestDataGeneration.ValueProviders.Context;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.ValueProviders
{
  //TODO: Write test showing the usage of this value provider
  //REVIEW: Is this ok?
  public abstract class RangedValueProvider<TMember> : ValueProvider<TMember, RangedValueProviderContext<TMember>>
  {
    protected override RangedValueProviderContext<TMember> CreateContext (ValueProviderObjectContext objectContext)
    {
      return new RangedValueProviderContext<TMember> (objectContext, CreateConstraints(objectContext));
    }

    protected abstract GenericRangeConstraints<TMember> CreateConstraints (ValueProviderObjectContext objectContext);
  }

  public abstract class AttributeBasedRangedValueProvider<TMember, TAttribute>:RangedValueProvider<TMember>
      where TAttribute : Attribute
  {
    protected override sealed GenericRangeConstraints<TMember> CreateConstraints(ValueProviderObjectContext objectContext)
    {
      var attribute = objectContext.Member?.GetCustomAttribute<TAttribute>();
      if (attribute == null)
        throw new ArgumentException ($"Did not find {nameof (TAttribute)} attribute on '{objectContext.Member}'.");

      return CreateConstraints (attribute);
    }

    protected abstract GenericRangeConstraints<TMember> CreateConstraints (TAttribute attribute);
  }

  public abstract class DirectRangedValueProvider<TMember>:RangedValueProvider<TMember>
  {
    private readonly TMember _from;
    private readonly TMember _to;

    protected DirectRangedValueProvider (TMember from, TMember to)
    {
      _from = from;
      _to = to;
    }

    protected override GenericRangeConstraints<TMember> CreateConstraints (ValueProviderObjectContext objectContext)
    {
      return new GenericRangeConstraints<TMember> (_from, _to);
    }
  }

  public class GenericRangeConstraints<TMember>
  {
    public TMember From { get; }
    public TMember To { get; }
    public GenericRangeConstraints (TMember from, TMember to)
    {
      From = from;
      To = to;
    }
  }

  public class RangedValueProviderContext<TMember> : ValueProviderContext<TMember>
  {
    public GenericRangeConstraints<TMember> RangeConstraints { get; }

    public RangedValueProviderContext ([NotNull] ValueProviderObjectContext objectContext, GenericRangeConstraints<TMember>  rangeConstraints)
        : base(objectContext)
    {
      RangeConstraints = rangeConstraints;
    }
  }
}
