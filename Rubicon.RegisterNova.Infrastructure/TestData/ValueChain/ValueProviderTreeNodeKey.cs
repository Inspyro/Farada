using System;

namespace Rubicon.RegisterNova.Infrastructure.TestData.ValueChain
{
  public class ValueProviderTreeNodeKey
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
      return Equals((ValueProviderTreeNodeKey) obj);
    }

    public ValueProviderTreeNodeKey(Type type, string name=null)
    {
      Type = type;
      Name = name;
    }

    public bool Equals (ValueProviderTreeNodeKey other)
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