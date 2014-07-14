using System;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  internal class ClassWithVariousMembers
  {
    public string PublicField;
    public string PublicMethod()
    {
      return string.Empty;
    }

    public string GetOnlyProperty { get; private set; }
  }
}
