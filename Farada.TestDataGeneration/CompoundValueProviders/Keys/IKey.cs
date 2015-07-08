using System;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
{
  /// <summary>
  /// A Key can describe a member chain, a type or and attribute
  /// So basically a key describes the filling chain for the <see cref="CompoundValueProvider"/>
  /// </summary>
  internal interface IKey:IEquatable<IKey>
  {
    [CanBeNull]
    IKey PreviousKey { get; }
    IKey CreateKey (IFastMemberWithValues member);

    Type Type { get; }
    [CanBeNull]
    IFastMemberWithValues Member { get; }
    int RecursionDepth { get; }
    IKey ChangeMemberType (Type newMemberType);
  }
}