using System;
using Farada.TestDataGeneration.ValueProviders.Context;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.ValueProviders
{
  //TODO: Write test showing the usage of this value provider
  //REVIEW: Is this ok?
  public abstract class RangedValueProvider<TMember> : ValueProvider<TMember, RangedValueProviderContext<TMember>>
  {
    private readonly Func<ValueProviderObjectContext, GenericRangeConstraints<TMember>> _constraintsProvider;

    public RangedValueProvider(Func<ValueProviderObjectContext, GenericRangeConstraints<TMember>> constraintsProvider)
    {
      _constraintsProvider = constraintsProvider;
    }

    protected override RangedValueProviderContext<TMember> CreateContext (ValueProviderObjectContext objectContext)
    {
      return new RangedValueProviderContext<TMember> (objectContext, _constraintsProvider (objectContext));
    }
  }

  public abstract class DirectRangedValueProvider<TMember>:RangedValueProvider<TMember>
  {
    public DirectRangedValueProvider (TMember from, TMember to)
        : base(ctx=>new GenericRangeConstraints<TMember>(from, to))
    {
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
