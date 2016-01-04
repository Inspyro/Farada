using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.FastReflection
{
  internal class FastMemberBase:IFastMemberInfo
  {
    private readonly List<Type> _attributeTypes;
    private readonly List<Attribute> _attributes;

    protected FastMemberBase (
        IMemberExtensionService memberExtensionService,
        [CanBeNull] Type containingType,
        [CanBeNull] Type memberType,
        string name,
        IEnumerable<Attribute> attributes)
    {
      if (containingType == null)
        throw new ArgumentException ("The containingType of member " + name + " was null. This is not allowed", nameof (containingType));

      if (memberType == null)
        throw new ArgumentException ("The memberType of member " + name + " was null. This is not allowed", nameof (memberType));

      Name = name;
      Type = memberType;

      _attributes = memberExtensionService.GetAttributesFor (containingType, memberType, name, attributes).ToList();
      _attributeTypes = _attributes.Select (a => a.GetType()).ToList();
    }

    public string Name { get; private set; }
    public Type Type { get; private set; }

    public IEnumerable<Type> Attributes
    {
      get { return _attributeTypes; }
    }

    [CanBeNull]
    public T GetCustomAttribute<T> () where T : Attribute
    {
      return GetCustomAttributes<T>().FirstOrDefault();
    }

    public IEnumerable<T> GetCustomAttributes<T>() where T : Attribute
    {
      return _attributes.Where(a => a is T).Cast<T>();
    }

    public bool IsDefined (Type type)
    {
      return _attributeTypes.Any(type.IsAssignableFrom);
    }
  }
}