using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.FastReflection
{
  public interface IMemberExtensionService
  {
    IEnumerable<Attribute> GetAttributesFor (Type containingType, Type memberType, string memberName, IEnumerable<Attribute> memberAttributes);
  }

  public class DefaultMemberExtensionService : IMemberExtensionService
  {
    public IEnumerable<Attribute> GetAttributesFor(Type containingType, [CanBeNull] Type memberType, string memberName, IEnumerable<Attribute> memberAttributes)
    {
      return memberAttributes;
    }
  }
}