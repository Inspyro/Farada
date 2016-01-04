using System;
using System.Collections.Generic;
using System.Linq;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.IntegrationTests.TestDomain;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using JetBrains.Annotations;
using TestFx.SpecK;
using Farada.TestDataGeneration.Fluent;

namespace Farada.TestDataGeneration.IntegrationTests
{
  [Subject (typeof (ITestDataGenerator), "Create_TypeExtensions")]
  class TestDataGeneratorTypeExtensionSpeck : TestDataGeneratorBaseSpeck
  {
    public TestDataGeneratorTypeExtensionSpeck ()
    {
      Specify (x =>
          TestDataGenerator.Create<ClassWithVeryPropertyToArgConversion> (MaxRecursionDepth, null))
          .Case ("should fill class according to custom parameter to property conversion", _ => _
              .Given (ValueProviderCustomConversion())
              .It ("should fill name through argument", x => x.Result.Name.Should ().Be ("NameThroughValue")));

      Specify (x =>
          TestDataGenerator.Create<ClassWithAttribute> (MaxRecursionDepth, null))
          .Case ("should fill class according to extended attributes", _ => _
              .Given (ValueProviderWithExtendedAttributes (new ClassWithAttribute.CoolIntAttribute(10)))
              .It ("should fill attributed int through extended attributes", x => x.Result.AttributedInt.Should ().Be (21)));
    }

    Context ValueProviderCustomConversion ()
    {
      return c => c.Given ("domain provider with custom parameter to property conversion", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
            .UseDefaults (false)
            .UseParameterToPropertyConversion (argName =>
            {
              if (argName == "value")
                return "Name";

              throw new NotSupportedException ("The argName " + argName + " is not supported.");
            })
            .For<ClassWithVeryPropertyToArgConversion> ().AddProvider (new DefaultInstanceValueProvider<ClassWithVeryPropertyToArgConversion> ())
            .For<ClassWithVeryPropertyToArgConversion> ().Select (cw => cw.Name).AddProvider (ctx => "NameThroughValue");
      })
          .Given (TestDataGeneratorContext ());
    }

    Context ValueProviderWithExtendedAttributes(Attribute additionalAttribute)
    {
      return c => c.Given("domain provider with custom member extension service", x =>
      {
        TestDataDomainConfiguration = configurator => configurator
            .UseDefaults (false)
            .UseMemberExtensionService (new CustomMemberExtensions (additionalAttribute))
            .For<ClassWithAttribute> ().AddProvider (new DefaultInstanceValueProvider<ClassWithAttribute> ())
            .For<ClassWithAttribute> ()
            .Select (cw => cw.AttributedInt)
            .AddProvider<ClassWithAttribute, int, ClassWithAttribute.CoolIntAttribute> (ctx => ctx.Attributes.Sum (a => a.Value));
      })
         .Given(TestDataGeneratorContext());
    }
  }

  class CustomMemberExtensions : IMemberExtensionService
  {
    readonly Attribute _additionalAttribute;

    public CustomMemberExtensions (Attribute additionalAttribute)
    {
      _additionalAttribute = additionalAttribute;
    }

    public IEnumerable<Attribute> GetAttributesFor ([CanBeNull] Type type, string memberName, IEnumerable<Attribute> memberAttributes)
    {
      foreach (var memberAttribute in memberAttributes)
        yield return memberAttribute;

      yield return _additionalAttribute;
    }
  }
}
