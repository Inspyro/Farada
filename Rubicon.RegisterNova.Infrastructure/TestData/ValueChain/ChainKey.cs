using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  internal class ChainKey
  {
    public Type Type { get; private set; }
    public string Name { get; private set; }

    public override bool Equals (object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;
      if (ReferenceEquals(this, obj))
        return true;
      if (obj.GetType() != GetType())
        return false;
      return Equals((ChainKey) obj);
    }

    public ChainKey(Type type, string name=null)
    {
      Type = type;
      Name = name;
    }

    public bool Equals (ChainKey other)
    {
      return Type == other.Type && string.Equals(Name, other.Name);
    }

    public override int GetHashCode ()
    {
      unchecked
      {
        return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
      }
    }
  }
}