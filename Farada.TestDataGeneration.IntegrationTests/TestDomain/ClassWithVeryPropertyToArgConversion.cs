
namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  public class ClassWithVeryPropertyToArgConversion
  {
    public string Name { get; private set; }

    public ClassWithVeryPropertyToArgConversion(string value)
    {
      Name = value;
    }
  }
}