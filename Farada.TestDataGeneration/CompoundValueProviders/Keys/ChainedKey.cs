using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;
using Remotion.Utilities;
using EqualityUtility = Farada.TestDataGeneration.Extensions.EqualityUtility;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
{
  /// <summary>
  /// A key that represents a type chain (Class.Property1.Property2...)
  /// </summary>
  internal class ChainedKey : IKey, IEquatable<ChainedKey>
  {
    private readonly Type _declaringType;
    private readonly IList<MemberKeyPart> _memberChain;

    private readonly MemberKeyPart _lastMember;
    private readonly Type _concreteDeclaringType;

    public IFastMemberWithValues Member
    {
      get { return _lastMember.Member; }
    }

    public int RecursionDepth { get; private set; }

    public IKey ChangeMemberType (Type newMemberType)
    {
      if (Member == null)
        throw new InvalidOperationException ("You cannot change the member type of a non-existing (null) member");

      var newMemberChain = new List<MemberKeyPart> (_memberChain);
      newMemberChain[newMemberChain.Count - 1] = new MemberKeyPart (Member, newMemberType);

      return new ChainedKey (_declaringType, newMemberChain);
    }

    public IKey PreviousKey { get; private set; }

    internal ChainedKey (Type declaringType, IFastMemberWithValues member)
        : this (declaringType, new List<MemberKeyPart> { new MemberKeyPart (member) })
    {
    }

    internal ChainedKey (Type declaringType, IList<MemberKeyPart> memberChain)
        : this (declaringType, declaringType, memberChain)
    {
    }

    private ChainedKey (Type declaringType, Type concreteDeclaringType, IList<MemberKeyPart> memberChain)
    {
      ArgumentUtility.CheckNotNull ("declaringType", declaringType);

      _declaringType = declaringType;
      _concreteDeclaringType = concreteDeclaringType;
      _memberChain = memberChain;

      _lastMember = memberChain.Last();

      RecursionDepth = _memberChain.Count (keyPart => keyPart.MemberType == _lastMember.MemberType);
      PreviousKey = CreatePreviousKey();
    }

    private IKey CreatePreviousKey ()
    {
      var baseType = _declaringType.BaseType;
      if (baseType != typeof (object) && baseType != typeof (ValueType) && baseType != null)
        return new ChainedKey (baseType, _concreteDeclaringType, _memberChain);

      var firstMemberKey = _memberChain[0];
      var previousProperties = _memberChain.Slice (1);

      if (previousProperties.Count == 0)
        return new TypeKey (firstMemberKey.MemberType);

      var previousDeclaringType = firstMemberKey.MemberType;
      return new ChainedKey (previousDeclaringType, previousProperties);
    }

    public IKey CreateKey (IFastMemberWithValues member)
    {
      return new ChainedKey (_declaringType, new List<MemberKeyPart> (_memberChain) { new MemberKeyPart (member) });
    }

    public Type Type
    {
      get { return _lastMember.MemberType; }
    }

    public bool Equals ([CanBeNull] ChainedKey other)
    {
      if (!EqualityUtility.ClassEquals (this, other))
        return false;

      Trace.Assert (other != null);
      if (_declaringType != other._declaringType)
        return false;

      if (_memberChain.Count != other._memberChain.Count)
        return false;

      return !_memberChain.Where ((t, i) => !t.Equals (other._memberChain[i])).Any();
    }

    public override bool Equals ([CanBeNull] object obj)
    {
      return Equals (obj as ChainedKey);
    }

    public override int GetHashCode ()
    {
      return _declaringType.GetHashCode() ^ Remotion.Utilities.EqualityUtility.GetRotatedHashCode (_memberChain);
    }

    public bool Equals ([CanBeNull] IKey other)
    {
      return Equals (other as ChainedKey);
    }

    public override string ToString ()
    {
      return "KEY on " + _declaringType + ": " + string.Join (" > ", _memberChain.Select (kp => kp.ToString()));
    }
  }
}