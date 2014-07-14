using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator))]
  class TestDataGeneratorExceptionsSpeck:TestDataGeneratorBaseSpeck
  {
      Context ConfigurationContext (TestDataDomainConfiguration config)
    {
      return c => c
      .Given ("domain with invalid configuration", x =>
      {
        TestDataDomainConfiguration = config;
      })
      .Given (TestDataGeneratorContext (catchExceptions:true));
    }

     /* Assertion<DontCare, ClassWithVariousMembers> Throws(Type type, string message)
      {
          return Exception.GetType () == type && Exception.Message == message;
      }*/

    [Group]
    void ExpressionExceptions ()
    {
        Specify (x => x.ToString ())
                .Elaborate ("should throw exception for fields", _ => _
                        .Given (ConfigurationContext (c => c.For ((ClassWithVariousMembers y) => y.PublicField).AddProvider (dummy => "")))
                        .It ("throws correct exception", x => CreationException.GetType ().Should ().Be (typeof (NotSupportedException)))
                        .It("throws exception with correct message", x => CreationException.Message.Should().Be("PublicField is not a property. Members that are not properties are not supported")))
                .Elaborate ("should throw exception for methods", _ => _
                        .Given (ConfigurationContext (c => c.For ((ClassWithVariousMembers y) => y.PublicMethod ()).AddProvider (dummy => "")))
                        .It ("throws correct exception", x => CreationException.GetType ().Should ().Be (typeof (NotSupportedException)))
                        .It("throws exception with correct message", x => CreationException.Message.Should().Be("A non parameter expression is not supported")))
                .Elaborate ("should throw exception for types", _ => _
                        .Given (ConfigurationContext (c => c.For ((ClassWithVariousMembers y) => y).AddProvider (dummy => null)))
                        .It("throws correct exception", x => CreationException.GetType().Should().Be(typeof(ArgumentException)))
                        .It("throws exception with correct message", x => CreationException.Message.Should().Be("Empty chains are not supported, please use AddProvider<T>()")));
    }


      [Group]
    void FillingExceptions()
    {
        Specify (x =>
                TestDataGenerator.Create<ClassWithVariousMembers> (MaxRecursionDepth, null))
                .Elaborate ("should throw exception for setting the value of a get only member", _ => _
                        .Given (ConfigurationContext (c => c.For ((ClassWithVariousMembers y) => y.GetOnlyProperty).AddProvider (dummy => "content")))
                        .It ("GetOnlyProperty should be null", x => x.Result.GetOnlyProperty.Should ().BeNull()));
    }
  }
}
