using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Farada.TestDataGeneration.FastReflection;
using FluentAssertions;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.UnitTests.FastReflection
{
  [Subject (typeof (FastReflectionUtility), "TODO")]
  public class FastReflectionSpeck:SpecK
  {
    Type TypeToReflect;
    PropertyInfo PropertyInfoToConvert;
    DerivedDTO Instance;

    public FastReflectionSpeck ()
    {
      Specify (x => FastReflectionUtility.GetTypeInfo (TypeToReflect))
          //
          .Case ("returns valid property info", _ => _
              //
              .Given ("SimpleDTO", x => TypeToReflect = typeof (SimpleDTO))
              //
              .It ("creates an valid type info", x => x.Result.Should ().NotBeNull ())
              .It ("finds 1 property", x => x.Result.Properties.Count.Should ().Be (1))
              .It ("finds correct property",
                  x => CompareType (x.Result, new KeyValuePair<Type, string> (typeof (bool), "SomeProperty"))))
          //
          .Case ("returns valid property info", _ => _
              //
              .Given ("DerivedDTO", x => TypeToReflect = typeof (DerivedDTO))
              //
              .It ("creates an valid type info", x => x.Result.Should ().NotBeNull ())
              .It ("finds 2 properties", x => x.Result.Properties.Count.Should ().Be (2))
              .It ("finds correct properties",
                  x =>
                      CompareType (x.Result, new KeyValuePair<Type, string> (typeof (int), "CustomProperty"),
                          new KeyValuePair<Type, string> (typeof (bool), "BaseProperty"))));


      Specify (x => FastReflectionUtility.GetPropertyInfo (PropertyInfoToConvert))
          //
          .Case ("returns valid property info", _ => _
              //
              .Given ("SimpleDTO.SomeProperty", x => PropertyInfoToConvert = TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<SimpleDTO, bool> (y => y.SomeProperty))
              //
              .It ("creates an valid type info", x => x.Result.Should ().NotBeNull ())
              .It ("finds correct property",
                  x => CompareProperty (x.Result, new KeyValuePair<Type, string> (typeof (bool), "SomeProperty"))))
          //
          .Case ("returns property info with valid getter and setter actions", _ => _
              //
              .Given ("SimpleDTO.SomeProperty", x => PropertyInfoToConvert =  TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<DerivedDTO, int> (y => y.CustomProperty))
              .Given ("SimpleDTO instance", x => Instance = new DerivedDTO { CustomProperty = 5 })
              //
              .It ("gets inital value", x => x.Result.GetValue (Instance).Should ().Be (5))
              .It ("sets value to instance", x =>
              {
                x.Result.SetValue (Instance, 100);
                Instance.CustomProperty.Should ().Be (100);
              })
              .It ("throws exception", x => x.Result.Invoking (c => c.SetValue (Instance, true)).ShouldThrow<InvalidCastException> ()))
          //
          .Case ("returns property info with valid attributes", _ => _
              //
              .Given ("AttributedDTO.AttributedProperty",
                  x => PropertyInfoToConvert =  TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<AttributedDTO, int> (y => y.AttributedProperty))
              //
              .It ("gets 2 attributes", x => x.Result.Attributes.Count ().Should ().Be (2))
              .It ("gets correct attributes types",
                  x => CompareAttributes (x.Result.Attributes.ToList (), typeof (CoolIntAttribute), typeof (FancyNumberAttribute))))
          //
          .Case ("returns property info with is defined valid", _ => _
              //
              .Given ("AttributedDTO.AttributedProperty",
                  x => PropertyInfoToConvert =  TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<AttributedDTO, int> (y => y.AttributedProperty))
              //
              .It ("gets cool int attribute", x => x.Result.IsDefined (typeof (CoolIntAttribute)).Should ().BeTrue ())
              .It ("gets fancy number attribute", x => x.Result.IsDefined (typeof (FancyNumberAttribute)).Should ().BeTrue ())
              .It ("does not get other attribute", x => x.Result.IsDefined (typeof (OtherAttribute)).Should ().BeFalse ()))
          //
          .Case ("returns concrete attributes", _ => _
              //
              .Given ("AttributedDTO.AttributedProperty",
                  x => PropertyInfoToConvert =  TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<AttributedDTO, int> (y => y.AttributedProperty))
              //
              .It ("returns value of cool int attribute", x => x.Result.GetCustomAttribute<CoolIntAttribute> ().Value.Should ().Be (5))
              .It ("returns fanciness of fancy number attribute",
                  x => x.Result.GetCustomAttribute<FancyNumberAttribute> ().Fanciness.Should ().Be (Fanciness.VeryFancy))
              .It ("does not get other attribute", x => x.Result.GetCustomAttribute<OtherAttribute> ().Should ().BeNull ()));
    }

    static void CompareAttributes (IList<Type> attributes, params Type[] expectedAttributes)
    {
      foreach (var expectedAttribute in expectedAttributes)
      {
        attributes.Should ().Contain (expectedAttribute);
      }
    }

    static void CompareType(IFastTypeInfo typeInfo, params KeyValuePair<Type,string >[] properties)
    {
      for(int i=0;i<typeInfo.Properties.Count;i++)
      {
        var propertyToCompare = properties[i];
        var property = typeInfo.Properties.SingleOrDefault (x => x.Name == propertyToCompare.Value);

        property.Should ().NotBeNull ("Did not find property with name "+propertyToCompare.Value);
        Trace.Assert (property != null);
        CompareProperty (property, propertyToCompare);
      }
    }

    static void CompareProperty (IFastPropertyInfo propertyInfo,KeyValuePair<Type,string > property)
    {
      propertyInfo.Type.Should ().Be (property.Key);
      propertyInfo.Name.Should ().Be (property.Value);
    }
  }

  class OtherAttribute:Attribute
  {
  }

  class SimpleDTO
  {
    public bool SomeProperty { get; set; }
  }

  class DerivedDTO:BaseDTO
  {
    public int CustomProperty { get; set; }
  }

  class BaseDTO
  {
    public bool BaseProperty { get; set; }
  }

  class AttributedDTO
  {
    [CoolInt(5)]
    [FancyNumber(Fanciness.VeryFancy)]
    public int AttributedProperty { get; set; }
  }

  class FancyNumberAttribute : Attribute
  {
    public Fanciness Fanciness { get; private set; }

    public FancyNumberAttribute(Fanciness fanciness)
    {
      Fanciness = fanciness;
    }
  }

  enum Fanciness
  {
    ALittleFancy,
    VeryFancy
  }

  class CoolIntAttribute : Attribute
  {
    public int Value { get; private set; }

    public CoolIntAttribute(int value)
    {
      Value = value;
    }
  }
}