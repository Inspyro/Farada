using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.FastReflection
{
  public interface IMemberExtensionService
  {
    IEnumerable<Attribute> GetAttributesFor ([CanBeNull] Type type, string memberName, IEnumerable<Attribute> memberAttributes);
  }

  public class DefaultMemberExtensionService : IMemberExtensionService
  {
    public IEnumerable<Attribute> GetAttributesFor([CanBeNull] Type type, string memberName, IEnumerable<Attribute> memberAttributes)
    {
      return memberAttributes;
    }
  }
}