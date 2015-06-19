using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.FastReflection
{
  internal class FastMemberBase:IFastMemberInfo
  {
    private readonly List<Type> _attributeTypes;
    private readonly List<Attribute> _attributes;

    protected FastMemberBase (string name, [CanBeNull] Type type, IEnumerable<Attribute> attributes)
    {
      Name = name;
      Type = type;

      _attributes = attributes.ToList();
      _attributeTypes = _attributes.Select (a => a.GetType()).ToList();
    }

    public string Name { get; private set; }
    public Type Type { get; private set; }

    public IEnumerable<Type> Attributes
    {
      get { return _attributeTypes; }
    }

    public T GetCustomAttribute<T> () where T : Attribute
    {
      return (T) _attributes.FirstOrDefault(a => a is T); 
    }

    public bool IsDefined (Type type)
    {
      return _attributeTypes.Any(type.IsAssignableFrom);
    }
  }
}