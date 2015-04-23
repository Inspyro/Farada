using System;
using Farada.TestDataGeneration.BaseDomain.Modifiers;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using FluentAssertions;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), "Create_WithConstraints")]
  class TestDataGeneratorConstraintsSpeck : TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorConstraintsSpeck ()
    {
      Specify (x =>
          TestDataGenerator.Create<ClassWithConstraints> (MaxRecursionDepth, null))
          .Case ("should fill", _ => _
              .Given (NullModifierContext ())
              .It ("long name considering MinLength attribute", x =>
                  x.Result.LongName.Length.Should ().BeGreaterOrEqualTo (10000))
              .It ("short name considering MaxLength attribute", x => x.Result.ShortName.Length.Should ().BeLessOrEqualTo (1))
              .It ("medium name considering Min and MaxLength attribute", x => x.Result.MediumName.Length.Should ().BeInRange (10, 20))
              .It ("ranged name considering StringLength attribute", x => x.Result.RangedName.Length.Should ().BeInRange (10, 100))
              .It ("ranged number considering Range attribute", x => x.Result.RangedNumber.Should ().BeInRange (10, 100))
              .It ("required prop considering Required attribute", x => x.Result.RequiredProp.Should ().NotBeNull ())
              .It ("other prop considering no attribute", x => x.Result.OtherProp.Should ().BeNull ()));

      Specify (x =>
          TestDataGenerator.Create<ClassWithInvalidStringLengthConstraint> (MaxRecursionDepth, null))
          .Case ("should raise argument out of range exception", _ => _
              .Given (BaseDomainContext ())
              .ItThrows (typeof(ArgumentOutOfRangeException)));//x=>"On the property System.String InvalidRangedName the StringLength attribute has an invalid range")); //TODO RN-246: contains??

      Specify (x =>
          TestDataGenerator.Create<ClassWithInvalidRangeConstraint> (MaxRecursionDepth, null))
          .Case ("should raise argument exception", _ => _
              .Given (BaseDomainContext ())
              .ItThrows (typeof(ArgumentOutOfRangeException)));
          //x => "On the property System.Int32 InvalidRangedNumber the Range attribute has an invalid range")); //TODO RN-246
    }

    Context NullModifierContext ()
    {
      return c => c
          .Given ("domain with null modifier", x =>
          {
            TestDataDomainConfiguration = configuration => configuration
                .UseDefaults (true)
                .AddInstanceModifier (new NullModifier (1));
          })
          .Given (TestDataGeneratorContext ());
    }
  }
}