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
  [Subject (typeof (ITestDataGenerator), "Create_WithEdgeCase")]
  class TestDataGeneratorEdgeCaseSpeck : TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorEdgeCaseSpeck ()
    {
      Specify (x => "empty")
          .Case ("should throw exception for methods", _ => _
              .Given (ConfigurationContext (c => c.For ((ClassWithVariousMembers y) => y.PublicMethod ()).AddProvider (dummy => "")))
              .It ("ex", x => CreationException.Should ().BeOfType<NotSupportedException> ())
              .It ("ex",
                  x => CreationException.Message.Should ().Be ("Empty chains / Non-member chains are not supported, please use AddProvider<T>()")))
          .Case ("should throw exception for types", _ => _
              .Given (ConfigurationContext (c => c.For ((ClassWithVariousMembers y) => y).AddProvider (dummy => null)))
              .It ("ex", x => CreationException.Should ().BeOfType<NotSupportedException> ())
              .It ("ex",
                  x => CreationException.Message.Should ().Be ("Empty chains / Non-member chains are not supported, please use AddProvider<T>()")));


      Specify (x =>
          TestDataGenerator.Create<ClassWithVariousMembers> (MaxRecursionDepth, null))
          .Case ("should throw exception for setting the value of a get only member", _ => _
              .Given (ConfigurationContext (c => c.For ((ClassWithVariousMembers y) => y.GetOnlyProperty).AddProvider (dummy => "content")))
              .It ("GetOnlyProperty should be null", x => x.Result.GetOnlyProperty.Should ().BeNull ()));


      Specify (x =>
          TestDataGenerator.Create<BaseClassWithProtectedProperty> (MaxRecursionDepth, null))
          .Case ("should use value provider for base type OverrideMe property and ignore sub type", _ => _
              .Given (NewPropertiesContext ())
              .It ("it assigns correct value", x => x.Result.OverrideMe.Should ().Be ("BaseValue")));

      Specify (x =>
          TestDataGenerator.Create<ClassOveridingPropertyWithNewType> (MaxRecursionDepth, null))
          .Case ("should use value provider for sub type and ignore base type", _ => _
              .Given (NewPropertiesContext ())
              .It ("it assigns correct value", x => x.Result.OverrideMe.Should ().Be (103)));

      Specify (x =>
          TestDataGenerator.Create<ClassOveridingPropertyWithNewType> (MaxRecursionDepth, null))
          .Case ("should ignore base type and use generic int provider", _ => _
              .Given (NewPropertiesBaseClassAndFixedInt ())
              .It ("it uses generic int provider", x => x.Result.OverrideMe.Should ().Be (3)));

      Specify (x =>
          TestDataGenerator.Create<ClassOveridingPropertyWithNewType> (MaxRecursionDepth, null))
          .Case ("when using previous value should ignore base type", _ => _
              .Given (NewPropertiesContextUsingPreviousValue ())
              .It ("it ignores base type and uses generic int provider", x => x.Result.OverrideMe.Should ().Be (4)));


      Specify (x =>
          TestDataGenerator.Create<ClassAddingAttributes> (MaxRecursionDepth, null))
          .Case ("should fill property with last added provider", _ => _
              .Given (AttributeFillerContext ())
              .It ("it assigns correct value", x => x.Result.SomeAttributedProperty.Should ().Be ("Subclass2")));


      Specify (x =>
          TestDataGenerator.Create<ClassAddingAttributes> (MaxRecursionDepth, null))
          .Case ("should fill property with other subclass attribute", _ => _
              .Given (AttributeConcreteForOtherContext ())
              .It ("it assigns correct value", x => x.Result.SomeAttributedProperty.Should ().Be ("Subclass1")));

      Specify (x =>
          TestDataGenerator.Create<ClassWithoutAttribute> (MaxRecursionDepth, null))
          .Case ("should fill property with previous provider value", _ => _
              .Given (AttributeMixedContext ())
              .It ("it assigns correct value", x => x.Result.PropertyWithoutAttribute.Should ().Be ("Some value")));

      Specify (x =>
          TestDataGenerator.Create<ClassWithoutAttribute> (MaxRecursionDepth, null))
          .Case ("should throw exception because of missing provider", _ => _
              .Given (AttributeMixedOtherWayContext ())
              .ItThrows (typeof (NotSupportedException),
                  "Could not auto-fill Farada.TestDataGeneration.IntegrationTests.TestDomain.ClassWithoutAttribute> " +
                  "(member PropertyWithoutAttribute). Please provide a value provider")
              .ItThrowsInner (typeof (MissingValueProviderException),
                  "Tried to call previous provider on KEY on Farada.TestDataGeneration.IntegrationTests.TestDomain.ClassWithoutAttribute: " +
                  "Type: String, Member: PropertyWithoutAttribute but no previous provider was registered. Are you missing a value provider registration?"));
    }

    Context ConfigurationContext (TestDataDomainConfiguration config)
    {
      return c => c
          .Given ("domain with invalid configuration", x => { TestDataDomainConfiguration = config; })
          .Given (TestDataGeneratorContext (catchExceptions: true));
    }

    Context NewPropertiesContext ()
    {
      return c => c
          .Given ("domain with invalid configuration", x =>
          {
            TestDataDomainConfiguration =
                configuration => configuration
                    .For ((BaseClassWithProtectedProperty b) => b.OverrideMe).AddProvider (context => "BaseValue")
                    .For ((ClassOveridingPropertyWithNewType b) => b.OverrideMe).AddProvider (context => 103);
          })
          .Given (TestDataGeneratorContext ());
    }

    Context NewPropertiesBaseClassAndFixedInt ()
    {
      return c => c
          .Given ("domain with invalid configuration", x =>
          {
            TestDataDomainConfiguration =
                configuration => configuration
                    .For<int> ().AddProvider (context => 3)
                    .For ((BaseClassWithProtectedProperty b) => b.OverrideMe).AddProvider (context => "BaseValue");
          })
          .Given (TestDataGeneratorContext ());
    }

    Context NewPropertiesContextUsingPreviousValue ()
    {
      return c => c
          .Given ("domain with invalid configuration", x =>
          {
            TestDataDomainConfiguration =
                configuration => configuration
                    .For<int> ().AddProvider (context => 3)
                    .For ((BaseClassWithProtectedProperty b) => b.OverrideMe).AddProvider (context => "BaseValue")
                    .For ((ClassOveridingPropertyWithNewType b) => b.OverrideMe).AddProvider (context => 1 + context.GetPreviousValue ());
          })
          .Given (TestDataGeneratorContext ());
    }

    Context AttributeFillerContext ()
    {
      return c => c
          .Given ("domain with providers configured for subclass string 1 and 2", x =>
          {
            TestDataDomainConfiguration = configuration => configuration
                .For<string> ()
                .AddProvider<string, SubClassString1Attribute> (context => context.AdditionalData[0].Content)
                .AddProvider<string, SubClassString2Attribute> (context => context.AdditionalData[0].Content);
          })
          .Given (TestDataGeneratorContext ());
    }

    Context AttributeConcreteForOtherContext ()
    {
      return c => c
          .Given ("domain with provider just for other attribute of subclass", x =>
          {
            TestDataDomainConfiguration = configuration => configuration
                .For<string> ()
                .AddProvider<string, SubClassString2Attribute> (context => context.AdditionalData[0].Content)
                .AddProvider<string, SubClassString1Attribute> (context => context.AdditionalData[0].Content);
          })
          .Given (TestDataGeneratorContext ());
    }

    Context AttributeMixedContext ()
    {
      return c => c
          .Given ("domain with providers configured for default string -> subclass string 1", x =>
          {
            TestDataDomainConfiguration = configuration => configuration
                .UseDefaults (false)
                .For<ClassWithoutAttribute> ().AddProvider (new DefaultInstanceValueProvider<ClassWithoutAttribute> ())
                .For<string> ()
                .AddProvider<string> (context => "Some value")
                .AddProvider<string, SubClassString1Attribute> (context => context.AdditionalData[0].Content);
          })
          .Given (TestDataGeneratorContext ());
    }

    Context AttributeMixedOtherWayContext ()
    {
      return c => c
          .Given ("domain with providers configured for subclass string 1 -> default string", x =>
          {
            TestDataDomainConfiguration = configuration => configuration
                .UseDefaults (false)
                .For<ClassWithoutAttribute> ().AddProvider (new DefaultInstanceValueProvider<ClassWithoutAttribute> ())
                .For<string> ()
                .AddProvider<string, SubClassString1Attribute> (context => context.AdditionalData[0].Content);
          })
          .Given (TestDataGeneratorContext ());
    }
  }
}