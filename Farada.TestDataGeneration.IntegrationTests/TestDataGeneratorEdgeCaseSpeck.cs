using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), "Create_WithEdgeCase")]
  class TestDataGeneratorEdgeCaseSpeck : TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorEdgeCaseSpeck ()
    {
      Specify (x => x.ToString ())
          //x => CreationException.Message.Should ().Be ("PublicField is not a property. Members that are not properties are not supported")))
          .Case ("should throw exception for methods", _ => _
              .Given (ConfigurationContext (c => c.For ((ClassWithVariousMembers y) => y.PublicMethod ()).AddProvider (dummy => "")))
              .It ("throws correct exception", x => CreationException.GetType ().Should ().Be (typeof (NotSupportedException)))
              .ItThrows (typeof(Exception))) //"throws exception with correct message", //TODO RN-246
          //x => CreationException.Message.Should ().Be ("A non parameter expression is not supported")))
          .Case ("should throw exception for types", _ => _
              .Given (ConfigurationContext (c => c.For ((ClassWithVariousMembers y) => y).AddProvider (dummy => null)))
              .It ("throws correct exception", x => CreationException.GetType ().Should ().Be (typeof (ArgumentException)))
              .ItThrows (typeof(Exception))); //"throws exception with correct message", //TODO RN-246
                  //x => CreationException.Message.Should ().Be ("Empty chains are not supported, please use AddProvider<T>()")));


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
          .Case ("should fill property with alphabetically first attribute", _ => _
              .Given (AttributeFillerContext ())
              .It ("it assigns correct value", x => x.Result.SomeAttributedProperty.Should ().Be ("Subclass1")));


      Specify (x =>
          TestDataGenerator.Create<ClassAddingAttributes> (MaxRecursionDepth, null))
          .Case ("should fill property with second subclass attribute", _ => _
              .Given (AttributeConcreteForSecondContext ())
              .It ("it assigns correct value", x => x.Result.SomeAttributedProperty.Should ().Be ("Subclass2")));
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
          .Given ("domain with providers configured for sublclass string 1 and 2", x =>
          {
            TestDataDomainConfiguration = configuration => configuration
                .For<string, SubClassString1Attribute> ().AddProvider (context => context.AdditionalData.Content)
                .For<string, SubClassString2Attribute> ().AddProvider (context => context.AdditionalData.Content);
          })
          .Given (TestDataGeneratorContext ());
    }

    Context AttributeConcreteForSecondContext ()
    {
      return c => c
          .Given ("domain with provider just for second attribute of subclass", x =>
          {
            TestDataDomainConfiguration = configuration => configuration
                .For<string, SubClassString2Attribute> ().AddProvider (context => context.AdditionalData.Content);
          })
          .Given (TestDataGeneratorContext ());
    }
  }
}