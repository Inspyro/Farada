using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using FluentAssertions;
using TestFx.Specifications;
using TestFx.Specifications.InferredApi;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), "TODO")]
  class TestDataGeneratorDomainSpeck:TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorDomainSpeck ()
    {
      //REVIEW RN-242: Not detected?!

      //default domain
       GenericCase<byte> ("simple byte case", _ => _
          .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random byte", x => x.Result.Should ().Be(244)));

      GenericCase<char> ("simple char case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random char", x => x.Result.Should ().Be(';')));

      GenericCase<decimal> ("simple decimal case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random decimal", x => x.Result.Should ().Be (-12805660511301768796771975167m)));

      GenericCase<double> ("simple double case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random double", x => x.Result.Should ().Be (-2.9056142737628133E+307)));

      GenericCase<SomeEnum> ("simple Enum case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a valid Enum", x => x.Result.Should ().Be(SomeEnum.SomeMember3)));

      GenericCase<float> ("simple float case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random float", x => x.Result.Should ().Be(-5.499989E+37f)));

       GenericCase<int> ("simple int case", _ => _
         .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random int", x => x.Result.Should ().Be(726643699)));

      GenericCase<long> ("simple long case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random long", x => x.Result.Should ().Be(-1490775089629665279L)));

       GenericCase<sbyte> ("simple sbyte case", _ => _
         .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random sbyte", x => x.Result.Should ().Be(-42)));

      GenericCase<short> ("simple short case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random short", x => x.Result.Should ().Be (-5297)));

      GenericCase<uint> ("simple uint case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random uint", x => x.Result.Should ().Be(726643700)));

      GenericCase<ulong> ("simple ulong case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random ulong", x => x.Result.Should ().Be(1453287401)));

      GenericCase<ushort> ("simple ushort case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should be a random ushort", x => x.Result.Should ().Be(11087)));

      GenericCase<string> ("simple string case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should equal test", x => x.Result.Should ().Be ("Homfu")));

      GenericCase<DateTime> ("simple DateTime case", _ => _
        .Given(BaseDomainContext(seed:5))
          //
          .It ("should equal some random past DateTime", x => x.Result.Should ().Be (new DateTime(620621813737538675))));

      //empty domain

       GenericCase<byte> ("simple byte case with empty domain", _ => _
         .Given(BaseDomainContext(false))
          //
          .It ("should be a valid byte", x => x.Result.Should ().Be(default(byte))));

      GenericCase<char> ("simple char case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid char", x => x.Result.Should ().Be(default(char))));

      GenericCase<decimal> ("simple decimal case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid decimal", x => x.Result.Should ().Be(default(decimal))));

      GenericCase<double> ("simple double case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid double", x => x.Result.Should ().Be(default(double))));

      GenericCase<SomeEnum> ("simple Enum case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid Enum", x => x.Result.Should ().Be(SomeEnum.SomeMember1)));

      GenericCase<float> ("simple float case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid float", x => x.Result.Should ().Be(default(float))));

       GenericCase<int> ("simple int case with empty domain", _ => _
         .Given(BaseDomainContext(false))
          //
          .It ("should be a valid int", x => x.Result.Should ().Be(default(int))));

      GenericCase<long> ("simple long case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid long", x => x.Result.Should ().Be(default(long))));

       GenericCase<sbyte> ("simple sbyte case with empty domain", _ => _
         .Given(BaseDomainContext(false))
          //
          .It ("should be a valid sbyte", x => x.Result.Should ().Be(default(sbyte))));

      GenericCase<short> ("simple short case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid short", x => x.Result.Should ().Be(default(short))));

      GenericCase<uint> ("simple uint case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid uint", x => x.Result.Should ().Be(default(uint))));

      GenericCase<ulong> ("simple ulong case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid ulong", x => x.Result.Should ().Be(default(ulong))));

      GenericCase<ushort> ("simple ushort case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should be a valid ushort", x => x.Result.Should ().Be(default(ushort))));

      GenericCase<string> ("simple string case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should equal test", x => x.Result.Should ().BeNull ()));

      GenericCase<DateTime> ("simple DateTime case with empty domain", _ => _
        .Given(BaseDomainContext(false))
          //
          .It ("should equal some DateTime", x => x.Result.GetType ().Should ().Be (typeof (DateTime))));
    }

     void GenericCase<T> (string caseDescription, Func<IDefineOrArrangeOrAssert<Dummy, T, object>, IAssert> succession)
    {
      Specify (x => TestDataGenerator.Create<T> (MaxRecursionDepth, null)).Case (caseDescription, succession);
    }
  }
}
