using System;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
{
  /// <summary>
  /// A Key can describe a property chain, a type or and attribute
  /// So basically a key describes the filling chain for the <see cref="CompoundValueProvider"/>
  /// </summary>
  internal interface IKey:IEquatable<IKey>
  {
    [CanBeNull]
    IKey PreviousKey { get; }
    IKey CreateKey (IFastPropertyInfo property);

    Type Type { get; }
    [CanBeNull]
    IFastPropertyInfo Property { get; }
    int RecursionDepth { get; }
    IKey ChangePropertyType (Type newPropertyType);
  }
}