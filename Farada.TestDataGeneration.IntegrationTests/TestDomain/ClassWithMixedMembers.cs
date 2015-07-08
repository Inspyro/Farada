using System;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class ClassWithMixedMembers
  {
    public string PublicProperty { get; set; }
    public string PublicField;

    string _privateProperty = "default";
    string _privateField = "default";

    public string GetPrivateProperty ()
    {
      return _privateProperty;
    }

    public string GetPrivateField ()
    {
      return _privateField;
    }
  }
}
