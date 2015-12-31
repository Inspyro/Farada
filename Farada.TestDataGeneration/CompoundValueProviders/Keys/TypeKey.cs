using System;
using System.Collections.Generic;
using System.Diagnostics;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
{
  /// <summary>
  /// A key that represents a type (e.g. type of a property)
  /// </summary>
  internal class TypeKey : IKey, IEquatable<TypeKey>
  {
    private readonly Type _type;
    private readonly Queue<Type> _interfaces;

    public TypeKey (Type type) : this(type, interfaces: GetInterfaces(type))
    {
    }

    internal static Queue<Type> GetInterfaces (Type type)
    {
      return new Queue<Type> (type.UnwrapIfNullable().GetInterfaces());
    }

    private TypeKey(Type type, Queue<Type> interfaces)
    {
      _type = type;
      _interfaces = interfaces;
      PreviousKey = CreatePreviousKey();
    }

    [CanBeNull]
    private IKey CreatePreviousKey ()
    {
      if (_type.IsUnwrappableNullableType())
       return new TypeKey(_type.UnwrapIfNullable());

      var baseType = _type.BaseType;
      if (baseType != null)
        return new TypeKey (baseType, _interfaces);

      //we go through all interfaces of the most concrete (not nullable) type.
      if (_interfaces.Count > 0)
        return new TypeKey (_interfaces.Dequeue(), _interfaces);

      return null;
    }

    public IKey PreviousKey { get; private set; }

    public IKey CreateKey (IFastMemberWithValues member)
    {
      return new ChainedKey(_type, new List<MemberKeyPart> { new MemberKeyPart(member) });
    }

    public Type Type
    {
      get { return _type; }
    }

    public IFastMemberWithValues Member
    {
      get { return null; }
    }

    public int RecursionDepth
    {
      get { return 0; }
    }

    public IKey ChangeMemberType (Type newMemberType)
    {
      return new TypeKey(newMemberType);
    }

    public override string ToString ()
    {
      return Type.FullName + (Member != null ? "." + Member : "");
    }

    public bool Equals ([CanBeNull] TypeKey other)
    {
      if (!EqualityUtility.ClassEquals(this, other))
        return false;

      Trace.Assert (other != null);
      return _type == other._type;
    }

    public bool Equals ([CanBeNull] IKey other)
    {
      return Equals(other as TypeKey);
    }

    public override bool Equals ([CanBeNull] object obj)
    {
      return Equals(obj as TypeKey);
    }

    public override int GetHashCode ()
    {
      return _type.GetHashCode();
    }
  }
}