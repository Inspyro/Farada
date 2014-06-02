using System;
using FakeItEasy;
using FluentAssertions;
using Rubicon.RegisterNova.Infrastructure.TestData;
using Rubicon.RegisterNova.Infrastructure.TestData.CompoundValueProvider;
using SpecK;
using SpecK.Extensibility;
using SpecK.Specifications;
using SpecK.Specifications.InferredApi;

namespace Rubicon.RegisterNova.Infrastructure.UnitTests.TestData.IntegrationTest
{
  [Subject (typeof (ICompoundValueProvider))]
  public class TestDataIntegrationSpeck : Specs
  {
    BaseDomainConfiguration Domain;
    bool UseDefaults;
    ICompoundValueProvider ValueProvider;
    int MaxRecursionDepth;

    [Group]
    void ValueProviderWithDefaultDomain ()
    {
      Domain = new BaseDomainConfiguration ();
      UseDefaults = true;
      ValueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider (Domain, UseDefaults);
      MaxRecursionDepth = 2;

        GenericCase<byte> ("simple byte case", _ => _
          //
          .It ("should be a valid byte", x => x.Result.Should ().BeInRange(byte.MinValue, byte.MaxValue)));

      GenericCase<char> ("simple char case", _ => _
          //
          .It ("should be a valid char", x => x.Result.Should ().BeInRange ((char) 50, (char) 300)));

      GenericCase<decimal> ("simple decimal case", _ => _
          //
          .It ("should be a valid decimal", x => x.Result.Should ().BeInRange (decimal.MinValue, decimal.MaxValue)));

      GenericCase<double> ("simple double case", _ => _
          //
          .It ("should be a valid double", x => x.Result.Should ().BeInRange (double.MinValue, double.MaxValue)));

      GenericCase<SomeEnum> ("simple Enum case", _ => _
          //
          .It ("should be a valid Enum", x => x.Result.Should ().BeOfType<SomeEnum>()));

      GenericCase<float> ("simple float case", _ => _
          //
          .It ("should be a valid float", x => x.Result.Should ().BeInRange(float.MinValue, float.MaxValue)));

       GenericCase<int> ("simple int case", _ => _
          //
          .It ("should be a valid int", x => x.Result.Should ().BeInRange(int.MinValue, int.MaxValue)));

      GenericCase<long> ("simple long case", _ => _
          //
          .It ("should be a valid long", x => x.Result.Should ().BeInRange (long.MinValue, long.MaxValue)));

       GenericCase<sbyte> ("simple sbyte case", _ => _
          //
          .It ("should be a valid sbyte", x => x.Result.Should ().BeInRange (sbyte.MinValue, sbyte.MaxValue)));

      GenericCase<short> ("simple short case", _ => _
          //
          .It ("should be a valid short", x => x.Result.Should ().BeInRange (short.MinValue, short.MaxValue)));

      GenericCase<uint> ("simple uint case", _ => _
          //
          .It ("should be a valid uint", x => x.Result.Should ().BeInRange (uint.MinValue, uint.MaxValue)));

      GenericCase<ulong> ("simple ulong case", _ => _
          //
          .It ("should be a valid ulong", x => x.Result.Should ().BeInRange (ulong.MinValue, ulong.MaxValue)));

      GenericCase<ushort> ("simple ushort case", _ => _
          //
          .It ("should be a valid ushort", x => x.Result.Should ().BeInRange (ushort.MinValue, ushort.MaxValue)));

      GenericCase<string> ("simple string case", _ => _
          //
          .It ("should equal test", x => x.Result.Should ().NotBeNull ()));

      GenericCase<DateTime> ("simple DateTime case", _ => _
          //
          .It ("should equal some past DateTime", x => x.Result.Should ().BeBefore (DateTime.Today)));
    }

    [Group]
    void ValueProviderWithEmptyDomain ()
    {
      Domain = new BaseDomainConfiguration ();
      UseDefaults = false;
      ValueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider (Domain, UseDefaults);
      MaxRecursionDepth = 2;

       GenericCase<byte> ("simple byte case", _ => _
          //
          .It ("should be a valid byte", x => x.Result.Should ().Be(default(byte))));

      GenericCase<char> ("simple char case", _ => _
          //
          .It ("should be a valid char", x => x.Result.Should ().Be(default(char))));

      GenericCase<decimal> ("simple decimal case", _ => _
          //
          .It ("should be a valid decimal", x => x.Result.Should ().Be(default(decimal))));

      GenericCase<double> ("simple double case", _ => _
          //
          .It ("should be a valid double", x => x.Result.Should ().Be(default(double))));

      GenericCase<SomeEnum> ("simple Enum case", _ => _
          //
          .It ("should be a valid Enum", x => x.Result.Should ().BeOfType<SomeEnum>()));

      GenericCase<float> ("simple float case", _ => _
          //
          .It ("should be a valid float", x => x.Result.Should ().Be(default(float))));

       GenericCase<int> ("simple int case", _ => _
          //
          .It ("should be a valid int", x => x.Result.Should ().Be(default(int))));

      GenericCase<long> ("simple long case", _ => _
          //
          .It ("should be a valid long", x => x.Result.Should ().Be(default(long))));

       GenericCase<sbyte> ("simple sbyte case", _ => _
          //
          .It ("should be a valid sbyte", x => x.Result.Should ().Be(default(sbyte))));

      GenericCase<short> ("simple short case", _ => _
          //
          .It ("should be a valid short", x => x.Result.Should ().Be(default(short))));

      GenericCase<uint> ("simple uint case", _ => _
          //
          .It ("should be a valid uint", x => x.Result.Should ().Be(default(uint))));

      GenericCase<ulong> ("simple ulong case", _ => _
          //
          .It ("should be a valid ulong", x => x.Result.Should ().Be(default(ulong))));

      GenericCase<ushort> ("simple ushort case", _ => _
          //
          .It ("should be a valid ushort", x => x.Result.Should ().Be(default(ushort))));

      GenericCase<string> ("simple string case", _ => _
          //
          .It ("should equal test", x => x.Result.Should ().BeNull ()));

      GenericCase<DateTime> ("simple DateTime case", _ => _
          //
          .It ("should equal some DateTime", x => x.Result.GetType ().Should ().Be (typeof (DateTime))));
    }

    public void GenericCase<T>(string caseDescription, Func<IAgainstOrArrangeOrAssert<DontCare, T>, IAssert<DontCare, T>> succession)
    {
      Specify (x => ValueProvider.Create<T> (MaxRecursionDepth, null)).Elaborate (caseDescription, succession);
    }
  }

  enum SomeEnum
  {
  }
}