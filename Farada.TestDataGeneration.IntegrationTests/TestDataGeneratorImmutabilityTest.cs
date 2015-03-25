using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using FluentAssertions;
using NUnit.Framework;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [TestFixture]
  public class TestDataGeneratorImmutabilityTest
  {
    ITestDataGenerator _sut;

    [SetUp]
    public void SetUp ()
    {
      _sut = TestDataGeneratorFactory.Create (cfg =>
          cfg.UseDefaults (true)
              .For ((Ice ice) => ice.Origin).AddProvider (f => "FixedOrigin") //IDEA - ForCtorArg("origin")
              .For ((Ice ice) => ice.Temperature).AddProvider (f => -100));
    }

    [Test]
    public void PropertiesAreInitialized()
    {
      var result=_sut.Create<Ice> ();

      result.Origin.Should ().Be ("FixedOrigin");
      result.Temperature.Should ().Be (-100);
    }
  }

  internal class Ice
  {
    private readonly string _origin;
    private readonly int _temperature;

    public string Origin { get { return _origin;} }
    public int Temperature { get { return _temperature; } }

    public Ice (string origin, int temperature)
    {
      _origin = origin;
      _temperature = temperature;
    }
  }
}