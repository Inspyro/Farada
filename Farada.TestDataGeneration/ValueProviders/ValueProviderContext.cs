using System;
using System.Linq.Expressions;
using System.Reflection;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.FastReflection;

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
    private readonly Func<DependedPropertyCollection, TMember> _previousValueFunction;
    private object _advanced;
    internal DependedPropertyCollection PropertyCollection { get; private set; }

    /// <summary>
    /// The random to use for random value generation
    /// </summary>
    public IRandom Random { get; private set; }

    /// <summary>
    /// The func that referes to the previous value generator in the chain
    /// </summary>
    public TMember GetPreviousValue ()
    {
      return _previousValueFunction(PropertyCollection);
    }

    /*public TType GetDependendValue<TType, TMember>(Expression<TType, TMember> expression)
    {
      return PropertyCollection[expression];
    }*/

    /// <summary>
    /// Enriches the context with a new DependendPropertyCollection.
    /// </summary>
    /// <param name="dependedPropertyCollection">Containing a mapping between dependend properties and values</param>
    public ValueProviderContext<TMember> Enrich(DependedPropertyCollection dependedPropertyCollection)
    {
      //after we enrich the context, we have a potentially filled property collection.
      PropertyCollection = dependedPropertyCollection;
      return this; //REVIEW: Should this be implemented as immutable (With method?)
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
      _previousValueFunction = (dependedPropertyCollection) => (TMember) objectContext.GetPreviousValue(dependedPropertyCollection);
      TargetValueType = objectContext.TargetValueType;
      Member = objectContext.Member;
      Advanced = objectContext.Advanced;

      //By default we have an empty property collection.
      PropertyCollection = new DependedPropertyCollection();
    }
  }
}