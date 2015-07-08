using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
{
  /// <summary>
  /// A key that represents an attribute on a member
  /// </summary>
  internal class AttributeKey : IKey, IEquatable<AttributeKey>
  {
    private readonly Type _type;
    private readonly Type _attributeType;

    private readonly Type _mostConcreteMemberType;
    private readonly List<Type> _remainingAttributes;

    internal AttributeKey(Type type, Type attributeType)
      : this(type, new List<Type> { attributeType})
    {
    }

    internal AttributeKey(Type type, List<Type> remainingAttributes)
        : this(type, type, remainingAttributes)
    {
      
    }

    private AttributeKey(Type type, Type mostConcreteMemberType, List<Type> remainingAttributes )
    {
      if (remainingAttributes.Count < 1)
        throw new ArgumentException("Cannot create attribute key with less then one remaining attribute");

        remainingAttributes.Sort (new AlphaNumericComparer());

      _type = type;
      _mostConcreteMemberType = mostConcreteMemberType;
      _attributeType = remainingAttributes.First();
      _remainingAttributes = remainingAttributes;

      PreviousKey = CreatePreviousKey(remainingAttributes);
    }

    private IKey CreatePreviousKey(List<Type> remainingAttributes)
    {
      if (remainingAttributes.Count == 1)
      {
        return new TypeKey(_mostConcreteMemberType);
      }

      if (_type.BaseType != null)
        return new AttributeKey(_type.BaseType, _mostConcreteMemberType, remainingAttributes);

      return new AttributeKey(_mostConcreteMemberType, _mostConcreteMemberType, remainingAttributes.Slice(1));
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
      return new AttributeKey(newMemberType, _remainingAttributes);
    }

    public bool Equals ( [CanBeNull] AttributeKey other)
    {
      if (!EqualityUtility.ClassEquals(this, other))
        return false;

      Trace.Assert (other != null);
      return _type == other._type && _attributeType == other._attributeType;
    }

    public bool Equals ([CanBeNull] IKey other)
    {
      return Equals(other as AttributeKey);
    }

    public override bool Equals ([CanBeNull] object obj)
    {
      return Equals(obj as AttributeKey);
    }

    public override int GetHashCode ()
    {
      var hashCode = (_attributeType.GetHashCode());
      hashCode = (hashCode * 397) ^ (_type.GetHashCode());
      return hashCode;
    }
  }
}