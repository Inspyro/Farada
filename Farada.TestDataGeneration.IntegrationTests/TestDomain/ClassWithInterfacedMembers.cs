
namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  public class ClassWithInterfacedMembers
  {
    public InterfacedClass InterfacedClass;
    public DerivedInterfacedClass DerivedInterfacedClass;
  }
  public interface IInterface
  {
    string Name { get; set; }
  }

  public class InterfacedClass : IInterface
  {
    

    public string Name { get; set; }
  }

  public class DerivedInterfacedClass : InterfacedClass
  {
    public string Value { get; set; }
  }
}
