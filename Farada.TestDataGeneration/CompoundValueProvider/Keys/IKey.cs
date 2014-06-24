using System;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.CompoundValueProvider.Keys
{
  /// <summary>
  /// TODO
  /// </summary>
  internal interface IKey:IEquatable<IKey>
  {
    IKey PreviousKey { get; }
    IKey CreateKey (IFastPropertyInfo property);

    Type PropertyType { get; }
    IFastPropertyInfo Property { get; }
    int RecursionDepth { get; }
    IKey ChangePropertyType (Type newPropertyType);
  }
}