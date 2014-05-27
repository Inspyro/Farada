using System;
using System.Collections.Generic;
using Rubicon.RegisterNova.Infrastructure.TestData.FastReflection;

namespace Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider.Keys
{
  internal class TypeKey : IKey, IEquatable<TypeKey>
  {
    private readonly Type _type;

    public TypeKey (Type type)
    {
      _type = type;
      PreviousKey = CreatePreviousKey();
    }

    private IKey CreatePreviousKey ()
    {
      var baseType = _type.BaseType;
      if (baseType != null)
        return new TypeKey(baseType);

      return null;
    }

    public IKey PreviousKey { get; private set; }

    public IKey CreateKey (IFastPropertyInfo property)
    {
      return new ChainedKey(_type, new List<PropertyKeyPart> { new PropertyKeyPart(property) });
    }

    public Type PropertyType
    {
      get { return _type; }
    }

    public IFastPropertyInfo Property
    {
      get { return null; }
    }

    public int RecursionDepth
    {
      get { return 0; }
    }

    public bool Equals (TypeKey other)
    {
      if (!Utilities.EqualityUtility.ClassEquals(this, other))
        return false;

      return _type == other._type;
    }

    public bool Equals (IKey other)
    {
      return Equals(other as TypeKey);
    }

    public override bool Equals (object obj)
    {
      return Equals(obj as TypeKey);
    }

    public override int GetHashCode ()
    {
      return _type.GetHashCode();
    }
  }
}