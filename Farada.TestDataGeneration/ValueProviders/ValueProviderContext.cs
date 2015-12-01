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
  public class ValueProviderContext<TMember>:IValueProviderContext
  {
    protected readonly ValueProviderObjectContext ObjectContext;

    private readonly Func<DependedPropertyCollection, TMember> _previousValueFunction;
    private object _advanced;

    /// <summary>
    /// The random to use for random value generation
    /// </summary>
    public IRandom Random { get; private set; }

    /// <summary>
    /// The func that referes to the previous value generator in the chain
    /// </summary>
    public TMember GetPreviousValue ()
    {
      return _previousValueFunction(null); //we don't have any dependend propertier here.
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


    /// <summary>
    /// Enriches the context with a new DependendPropertyCollection.
    /// </summary>
    /// <param name="dependedPropertyCollection">Containing a mapping between dependend properties and values</param>
    public virtual TContext Enrich<TContext> (DependedPropertyCollection dependedPropertyCollection)
        where TContext : ValueProviderContext<TMember>
    {
      return (TContext) this;
    }

    protected internal ValueProviderContext (ValueProviderObjectContext objectContext)
    {
      ObjectContext=objectContext;

      TestDataGenerator = objectContext.TestDataGenerator;
      Random = objectContext.Random;
      _previousValueFunction = (dependedPropertyCollection) => (TMember) objectContext.GetPreviousValue(dependedPropertyCollection);
      TargetValueType = objectContext.TargetValueType;
      Member = objectContext.Member;
      Advanced = objectContext.Advanced;
    }
  }

  public class ValueProviderContext<TContainer, TMember> : ValueProviderContext<TMember>
  {
    internal DependedPropertyCollection PropertyCollection { get; private set; }
    
    public ValueProviderContext ([NotNull] ValueProviderObjectContext objectContext)
        : base(objectContext)
    {
    }

    public TDependedMember GetDependendValue<TDependedMember>(Expression<Func<TContainer, TDependedMember>> memberExpression)
    {
      var key = ChainedKey.FromExpression(memberExpression);
      if (!PropertyCollection.ContainsKey(key))
        throw new ArgumentException("Could not find key:'" + key + "' in dependend property collection. Have you registered the dependency?");

      return (TDependedMember)PropertyCollection[key];
    }

    public override TContext Enrich<TContext> (DependedPropertyCollection dependedPropertyCollection)
    {
      PropertyCollection = dependedPropertyCollection;
      return (TContext) (object) this;
    }
  }
}