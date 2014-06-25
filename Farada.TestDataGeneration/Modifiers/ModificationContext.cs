using System;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.Modifiers
{
  /// <summary>
  /// The context under which a modification in  an <see cref="IInstanceModifier"/> should happen
  /// </summary>
  public class ModificationContext
  {
    /// <summary>
    /// The type of the property to modify
    /// </summary>
    public Type PropertyType { get; private set; }

    /// <summary>
    /// The concrete property to modify - can be null
    /// </summary>
    public IFastPropertyInfo Property { get; private set; }

    /// <summary>
    /// The random to use for the modification
    /// </summary>
    public Random Random { get; private set; }

    internal ModificationContext (Type propertyType, IFastPropertyInfo property, Random random)
    {
      PropertyType = propertyType;
      Property = property;
      Random = random;
    }
  }
}