using System;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator))]
  class TestDataGeneratorEdgeCaseSpeck:TestDataGeneratorBaseSpeck
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

      Context NewPropertiesContext()
      {
          return c => c
          .Given("domain with invalid configuration", x =>
          {
              TestDataDomainConfiguration =
                      configuration => configuration
                              .For ((BaseClassWithProtectedProperty b) => b.OverrideMe).AddProvider (context => "BaseValue")
                              .For ((ClassOveridingPropertyWithNewType b) => b.OverrideMe).AddProvider (context => 103);
          })
          .Given(TestDataGeneratorContext());
      }

      Context NewPropertiesBaseClassAndFixedInt()
      {
          return c => c
          .Given("domain with invalid configuration", x =>
          {
              TestDataDomainConfiguration =
                      configuration => configuration
                              .For<int> ().AddProvider (context => 3)
                              .For ((BaseClassWithProtectedProperty b) => b.OverrideMe).AddProvider (context => "BaseValue");
          })
          .Given(TestDataGeneratorContext());
      }

      Context NewPropertiesContextUsingPreviousValue()
      {
          return c => c
          .Given("domain with invalid configuration", x =>
          {
              TestDataDomainConfiguration =
                      configuration => configuration
                          .For<int>().AddProvider(context => 3)
                              .For((BaseClassWithProtectedProperty b) => b.OverrideMe).AddProvider(context => "BaseValue")
                              .For((ClassOveridingPropertyWithNewType b) => b.OverrideMe).AddProvider(context => 1+context.GetPreviousValue());
          })
          .Given(TestDataGeneratorContext());
      }

      [Group]
      void NewProperties ()
      {
          Specify(x =>
                 TestDataGenerator.Create<BaseClassWithProtectedProperty>(MaxRecursionDepth, null))
                 .Elaborate("should use value provider for base type OverrideMe property and ignore sub type", _ => _
                         .Given(NewPropertiesContext())
                         .It("it assigns correct value", x => x.Result.OverrideMe.Should().Be("BaseValue")));

          Specify(x =>
                  TestDataGenerator.Create<ClassOveridingPropertyWithNewType>(MaxRecursionDepth, null))
                  .Elaborate("should use value provider for sub type and ignore base type", _ => _
                          .Given(NewPropertiesContext())
                          .It("it assigns correct value", x => x.Result.OverrideMe.Should().Be(103)));

          Specify(x =>
                  TestDataGenerator.Create<ClassOveridingPropertyWithNewType>(MaxRecursionDepth, null))
                  .Elaborate("should ignore base type and use generic int provider", _ => _
                          .Given(NewPropertiesBaseClassAndFixedInt())
                          .It("it uses generic int provider", x => x.Result.OverrideMe.Should().Be(3)));

          Specify(x =>
                  TestDataGenerator.Create<ClassOveridingPropertyWithNewType>(MaxRecursionDepth, null))
                  .Elaborate("when using previous value should ignore base type", _ => _
                          .Given(NewPropertiesContextUsingPreviousValue())
                          .It("it ignores base type and uses generic int provider", x => x.Result.OverrideMe.Should().Be(4)));
      }

      Context AttributeFillerContext ()
      {
          return c => c
              .Given("domain with providers configured for sublclass string 1 and 2", x =>
              {
                  TestDataDomainConfiguration = configuration=>configuration
                      .For<string, SubClassString1Attribute>().AddProvider(context => context.Attribute.Content)
                      .For<string, SubClassString2Attribute>().AddProvider(context => context.Attribute.Content);
              })
              .Given(TestDataGeneratorContext());
      
      }

      [Group]
      void AttributeOrder ()
      {
          Specify (x =>
                  TestDataGenerator.Create<ClassAddingAttributes>(MaxRecursionDepth, null))
                  .Elaborate ("should fill property with alphabetically first attribute", _ => _
                          .Given(AttributeFillerContext())
                          .It("it assigns correct value", x => x.Result.SomeAttributedProperty.Should().Be("Subclass1")));
      }

      Context AttributeConcreteForSecondContext()
      {
          return c => c
              .Given("domain with provider just for second attribute of subclass", x =>
              {
                  TestDataDomainConfiguration = configuration => configuration
                          .For<string, SubClassString2Attribute> ().AddProvider (context => context.Attribute.Content);
              })
              .Given(TestDataGeneratorContext());

      }

      [Group]
      void AttributePropertyBaseType()
      {
          Specify (x =>
                  TestDataGenerator.Create<ClassAddingAttributes> (MaxRecursionDepth, null))
                  .Elaborate ("should fill property with second subclass attribute", _ => _
                          .Given (AttributeConcreteForSecondContext ())
                          .It ("it assigns correct value", x => x.Result.SomeAttributedProperty.Should ().Be ("Subclass2")));
      }
  }
}
