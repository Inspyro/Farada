using System;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class ClassWithAttribute
  {
    [CoolInt(11)]
    public int AttributedInt { get; set; }

    internal class CoolIntAttribute : Attribute
    {
      internal int Value { get; private set; }

      public CoolIntAttribute(int value)
      {
        Value = value;
      }
    }
  }
}