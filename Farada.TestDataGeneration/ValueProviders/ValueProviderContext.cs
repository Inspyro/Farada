using System;
using System.Linq.Expressions;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// The concrete value provider context
  /// </summary>
  public interface IValueProviderContext
  {
  }

  /// <summary>
  /// The type safe value provider context used to create values in a <see cref="IValueProvider"/>
  /// </summary>
  /// <typeparam name="TMember"></typeparam>
  public class ValueProviderContext<TMember> : IValueProviderContext
  {
    private readonly Func<object, TMember> _previousValueFunction;
    public object InternalMetadata { get; private set; }

    /// <summary>
    /// The random to use for random value generation
    /// </summary>
    public IRandom Random { get; private set; }

    /// <summary>
    /// The func that referes to the previous value generator in the chain
    /// </summary>
    public TMember GetPreviousValue ()
    {
      return _previousValueFunction(InternalMetadata);
    }

    /// <summary>
    /// The type of the value to generate
    /// </summary>
    public Type TargetValueType { get; private set; }

    /// <summary>
    /// The member for which the value will be generated - can be null
    /// </summary>
    public IFastMemberWithValues Member { get; private set; }

    /// <summary>
    /// The TestDataGenerator that can be used to generate values of other types (Note: avoid circular dependencies)
    /// </summary>
    public ITestDataGenerator TestDataGenerator { get; private set; }

    /// <summary>
    /// Advanced context values.
    /// </summary>
    public ValueProviderObjectContext.AdvancedContext Advanced { get; private set; }

    protected internal ValueProviderContext (ValueProviderObjectContext objectContext)
    {
      TestDataGenerator = objectContext.TestDataGenerator;
      Random = objectContext.Random;
      _previousValueFunction = (metadata) => (TMember) objectContext.GetPreviousValue(metadata);
      TargetValueType = objectContext.TargetValueType;
      Member = objectContext.Member;
      Advanced = objectContext.Advanced;
    }

    //REVIEW: Make this method immutable?
    internal virtual TContext Enrich<TContext> (object metadata) where TContext : ValueProviderContext<TMember>
    {
      if (metadata == null)
      {
        throw new InvalidOperationException(
            "Cannot convert value provider for " + Advanced.Key + " because there was no metadata found");
      }


      InternalMetadata = metadata;
      return (TContext) this;
    }
  }
}