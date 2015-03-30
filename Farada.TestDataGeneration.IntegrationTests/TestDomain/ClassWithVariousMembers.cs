using System;
using JetBrains.Annotations;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
  internal class ClassWithVariousMembers
  {
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public string PublicField;
    public string PublicMethod()
    {
      return string.Empty;
    }

    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public string GetOnlyProperty { get; private set; }
  }
}
