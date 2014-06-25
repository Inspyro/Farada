using System;
using Farada.TestDataGeneration.BaseDomain.Modifiers;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
  class TestDataGeneratorConstraintsSpeck:TestDataGeneratorBaseSpeck
  {
     Context NullModifierContext ()
    {
      return c => c
      .Given ("domain with null modifier", x =>
      {
        TestDataDomainConfiguration = configuration => configuration 
          .UseDefaults(false)
          .AddInstanceModifier (new NullModifier (1));
      })
      .Given (TestDataGeneratorContext ());
    }

    [Group]
    void ValueProviderConstraints ()
    {
      Specify (x =>
          TestDataGenerator.Create<ClassWithConstraints> (MaxRecursionDepth, null))
          .Elaborate ("should fill", _ => _
              .Given (NullModifierContext ())
              .It ("long name considering MinLength attribute", x => 
                x.Result.LongName.Length.Should ().BeGreaterOrEqualTo (10000))
              .It ("short name considering MaxLength attribute", x => x.Result.ShortName.Length.Should ().BeLessOrEqualTo (1))
              .It ("medium name considering Min and MaxLength attribute", x => x.Result.MediumName.Length.Should ().BeInRange (10, 20))
              .It("ranged name considering StringLength attribute", x=>x.Result.RangedName.Length.Should().BeInRange(10,100))
              .It ("ranged number considering Range attribute", x => x.Result.RangedNumber.Should ().BeInRange (10, 100))
              .It ("required prop considering Required attribute", x => x.Result.RequiredProp.Should ().NotBeNull ())
              .It ("other prop considering no attribute", x => x.Result.OtherProp.Should ().BeNull ()));
    }

    [Group]
    void ValueProviderWithInvalidConstraints ()
    {
      Specify (x =>
          TestDataGenerator.Create<ClassWithInvalidStringLengthConstraint> (MaxRecursionDepth, null))
          .Elaborate ("should raise argument out of range exception", _ => _
              .Given (BaseDomainContext ())
              .ItThrows (typeof (ArgumentOutOfRangeException)
              )
              .It ("has correct message",
                  x =>
                      x.Exception.Message.Should ()
                          .Contain ("On the property System.String InvalidRangedName the StringLength attribute has an invalid range")));

      Specify (x =>
          TestDataGenerator.Create<ClassWithInvalidRangeConstraint> (MaxRecursionDepth, null))
          .Elaborate ("should raise argument exception", _ => _
              .Given (BaseDomainContext ())
              .ItThrows (typeof (ArgumentOutOfRangeException))
              .It ("has correct message",
                  x =>
                      x.Exception.Message.Should ()
                          .Contain (
                              "On the property System.Int32 InvalidRangedNumber the Range attribute has an invalid range")));
    }
  }
}
