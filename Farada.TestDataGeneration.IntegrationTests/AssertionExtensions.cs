using System;
using TestFx.Specifications.Implementation.Controllers;
using TestFx.Specifications.InferredApi;

namespace Farada.TestDataGeneration.IntegrationTests
{
  public static class AssertionExtensions
  {
    public static IAssert ContainsMessage (
        this IAssert assertion,
        string message)
    {
      assertion.Get<ITestController<Dummy,Dummy,Dummy, Dummy>> ().AddAssertion ("Contains Message", x =>
      {
        if (!x.Exception.Message.Contains (message))
          throw new Exception (string.Format ("Exception message '{0}' did not fullfill condition.", x.Exception.Message));
      }, true);

      return assertion;
    }
  }
}