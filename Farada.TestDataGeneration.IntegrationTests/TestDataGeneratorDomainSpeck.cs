using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.Exceptions;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using Farada.TestDataGeneration.IntegrationTests.Utils;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), "Create_WithCustomDomain")]
  class TestDataGeneratorDomainSpeck:TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorDomainSpeck ()
    {
      //default domain
      Specify (x => Create<byte> ()).Case ("simple byte case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random byte", x => x.Result.Should ().Be (244)));

      Specify (x => Create<char> ()).Case ("simple char case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random char", x => x.Result.Should ().Be (';')));

      Specify (x => Create<decimal> ()).Case ("simple decimal case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random decimal", x => x.Result.Should ().Be (-12805660511301768796771975167m)));

      Specify (x => Create<double> ()).Case ("simple double case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random double", x => x.Result.Should ().Be (-2.9056142737628133E+307)));

      Specify (x => Create<SomeEnum> ()).Case ("simple Enum case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a valid Enum", x => x.Result.Should ().Be (SomeEnum.SomeMember3)));

      Specify (x => Create<float> ()).Case ("simple float case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random float", x => x.Result.Should ().Be (-5.499989E+37f)));

      Specify (x => Create<int> ()).Case ("simple int case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random int", x => x.Result.Should ().Be (726643699)));

      Specify (x => Create<long> ()).Case ("simple long case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random long", x => x.Result.Should ().Be (-1490775089629665279L)));

      Specify (x => Create<sbyte> ()).Case ("simple sbyte case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random sbyte", x => x.Result.Should ().Be (-42)));

      Specify (x => Create<short> ()).Case ("simple short case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random short", x => x.Result.Should ().Be (-5297)));

      Specify (x => Create<uint> ()).Case ("simple uint case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random uint", x => x.Result.Should ().Be (726643700)));

      Specify (x => Create<ulong> ()).Case ("simple ulong case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random ulong", x => x.Result.Should ().Be (1453287401)));

      Specify (x => Create<ushort> ()).Case ("simple ushort case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random ushort", x => x.Result.Should ().Be (11087)));

      Specify (x => Create<Guid> ()).Case ("simple Guid case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should be a random Guid", x => x.Result.Should ().Be (Guid.Parse ("cf85ddf4-1ece-d1e2-3171-650938abd2b7"))));

      Specify (x => Create<string> ()).Case ("simple string case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should equal test", x => x.Result.Should ().Be ("Homfu")));

      Specify (x => Create<DateTime> ()).Case ("simple DateTime case", _ => _
          .Given (BaseDomainContext (seed: 5))
          //
          .It ("should equal some random past DateTime", x => x.Result.Should ().Be (new DateTime (620621813737538675))));

      Specify (x => Create<ClassWithEmail> ()).Case ("email case", _ => _
          .Given (BaseDomainContext (seed: 2))
          //
          .It ("should equal some random email address", x => x.Result.Email.Should ().Be ("lazdjv.bobb@lesg.ca")));


      //empty domain

      Specify (x => Create<byte> ()).Case ("simple byte case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.Byte>\""));

      Specify (x => Create<char> ()).Case ("simple char case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.Char>\""));

      Specify (x => Create<decimal> ()).Case ("simple decimal case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.Decimal>\""));

      Specify (x => Create<double> ()).Case ("simple double case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.Double>\""));

      Specify (x => Create<SomeEnum> ()).Case ("simple Enum case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException),
              "No value provider registered for \"Farada.TestDataGeneration.IntegrationTests.TestDomain.SomeEnum>\""));

      Specify (x => Create<float> ()).Case ("simple float case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.Single>\""));

      Specify (x => Create<int> ()).Case ("simple int case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.Int32>\""));

      Specify (x => Create<long> ()).Case ("simple long case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.Int64>\""));

      Specify (x => Create<sbyte> ()).Case ("simple sbyte case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.SByte>\""));

      Specify (x => Create<short> ()).Case ("simple short case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.Int16>\""));

      Specify (x => Create<uint> ()).Case ("simple uint case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.UInt32>\""));

      Specify (x => Create<ulong> ()).Case ("simple ulong case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.UInt64>\""));

      Specify (x => Create<ushort> ()).Case ("simple ushort case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.UInt16>\""));

      Specify (x => Create<Guid> ()).Case ("simple Guid case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.Guid>\""));

      Specify (x => Create<string> ()).Case ("simple string case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.String>\""));

      Specify (x => Create<DateTime> ()).Case ("simple DateTime case with empty domain", _ => _
          .Given (BaseDomainContext (useDefaults: false))
          //
          .ItThrowsContains (typeof (MissingValueProviderException), "No value provider registered for \"System.DateTime>\""));
      Specify (x => Create<ClassWithEmail> ()).Case ("email case with empty domain", _ => _
          .Given (SimpleStringDomain ("simple string"))
          //
          .It ("should fill simple string", x => x.Result.Email.Should ().Be ("simple string")));
    }

    Context SimpleStringDomain (string value)
    {
      return c => c.Given ("instance only domain", x =>
      {
        TestDataDomainConfiguration = configurator => configurator.UseDefaults (false)
            .For<object> ().AddProvider (new DefaultInstanceValueProvider<object> ())
            .For<string> ().AddProvider (ctx=>value);
      })
          .Given (TestDataGeneratorContext ());
    }

    T Create<T> ()
    {
      return TestDataGenerator.Create<T> (MaxRecursionDepth, null);
    }
  }
}
