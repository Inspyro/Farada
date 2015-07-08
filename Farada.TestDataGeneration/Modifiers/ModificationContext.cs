using System;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.Modifiers
{
  /// <summary>
  /// The context under which a modification in  an <see cref="IInstanceModifier"/> should happen
  /// </summary>
  public class ModificationContext
  {
    /// <summary>
    /// The type of the member to modify
    /// </summary>
    public Type MemberType { get; private set; }

    /// <summary>
    /// The concrete member to modify
    /// </summary>
    [CanBeNull]
    public IFastMemberWithValues Member { get; private set; }

    /// <summary>
    /// The random to use for the modification
    /// </summary>
    public Random Random { get; private set; }

    internal ModificationContext (Type memberType, [CanBeNull] IFastMemberWithValues member, Random random)
    {
      MemberType = memberType;
      Member = member;
      Random = random;
    }
  }
}