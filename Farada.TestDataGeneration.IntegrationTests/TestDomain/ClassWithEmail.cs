using System.ComponentModel.DataAnnotations;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  class ClassWithEmail
  {
    [EmailAddress]
    public string Email { get; set; }
  }
}