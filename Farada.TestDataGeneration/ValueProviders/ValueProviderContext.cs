using System;
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
    private readonly Func<TMember> _previousValueFunction;

    /// <summary>
    /// The random to use for random value generation
    /// </summary>
    public Random Random { get; private set; }

    /// <summary>
    /// The func that referes to the previous value generator in the chain
    /// </summary>
    public TMember GetPreviousValue ()
    {
      return _previousValueFunction();
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

    protected internal ValueProviderContext (ValueProviderObjectContext objectContext)
    {
      TestDataGenerator = objectContext.TestDataGenerator;
      Random = objectContext.Random;
      _previousValueFunction = () => (TMember) objectContext.GetPreviousValue();
      TargetValueType = objectContext.TargetValueType;
      Member = objectContext.MemberInfo;
    }
  }
}