using System;
using System.Collections.Generic;
using AutoMapper.Internal;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
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
      if (_type.IsNullableType())
      {
        var typeOfNullable = _type.GetTypeOfNullable();

        if (typeOfNullable != null)
          return new TypeKey(typeOfNullable);
      }

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

    public IKey ChangePropertyType (Type newPropertyType)
    {
      return new TypeKey(newPropertyType);
    }

    public bool Equals (TypeKey other)
    {
      if (!EqualityUtility.ClassEquals(this, other))
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