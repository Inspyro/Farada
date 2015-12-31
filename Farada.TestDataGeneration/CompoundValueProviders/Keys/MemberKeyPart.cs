using System;
using System.Diagnostics;
using Farada.TestDataGeneration.Extensions;
using Farada.TestDataGeneration.FastReflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.CompoundValueProviders.Keys
{
  /// <summary>
  ///  A key that represents a single member with value (property or field)
  /// </summary>
  internal class MemberKeyPart: IEquatable<MemberKeyPart>
  {
    public Type MemberType { get; private set; }
    public IFastMemberWithValues Member { get; private set; }

    internal MemberKeyPart (IFastMemberWithValues member, Type concreteType=null)
    {
      Member = member;
      MemberType = concreteType ?? Member.Type;
    }

    public override string ToString ()
    {
      return $"{Member.Name}";
    }

    public bool Equals ([CanBeNull] MemberKeyPart other)
    {
      if (!EqualityUtility.ClassEquals(this, other))
        return false;

      Trace.Assert (other != null);
      return Member.Name == other.Member.Name && MemberType == other.MemberType;
    }

    public override bool Equals ([CanBeNull] object obj)
    {
      return Equals(obj as MemberKeyPart);
    }

    public override int GetHashCode ()
    {
      unchecked
      {
        var hashCode = (Member.Name.GetHashCode());
        hashCode = (hashCode * 397) ^ (MemberType.GetHashCode());
        return hashCode;
      }
    }
  }
}