using System;
using Farada.TestDataGeneration.Extensions;
using FluentAssertions;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.UnitTests.Extensions
{
  [Subject (typeof (TestDataGeneration.Extensions.TypeExtensions), "TODO")]
  public class TypeExtensionsSpeck : SpecK<Type>
  {
    bool IncludeNonPublicConstructor;

    public TypeExtensionsSpeck ()
    {
      Specify (x => x.IsNullableType ())
          .Case ("simple nullable", _ => _
              .GivenSubject ("int?", x => typeof (int?))
              .It ("should be nullable", x => x.Result.Should ().BeTrue ()))
          .Case ("complex nullable", _ => _
              .GivenSubject ("Nullable<>", x => typeof (Nullable<>))
              .It ("should be nullable", x => x.Result.Should ().BeTrue ()))
              .Case ("non nullable", _ => _
              .GivenSubject ("int", x => typeof (int))
              .It ("should not be nullable", x => x.Result.Should ().BeFalse ()))
          .Case ("nullable class", _ => _
              .GivenSubject ("string", x => typeof (string))
              .It ("should not be nullable", x => x.Result.Should ().BeFalse ()));


      Specify (x => x.GetTypeOfNullable ())
          .Case ("simple nullable", _ => _
              .GivenSubject ("int?", x => typeof (int?))
              .It ("should be int", x => x.Result.Should ().Be (typeof (int))))
          .Case ("complex nullable", _ => _
              .GivenSubject ("Nullable<>", x => typeof (Nullable<>))
              .It ("should not be empty", x => x.Result.Should ().NotBeNull()))
          .Case ("non nullable", _ => _
              .GivenSubject ("int", x => typeof (int))
              .ItThrows<ArgumentException> ())
          .Case ("nullable class", _ => _
              .GivenSubject ("string", x => typeof (string))
              .ItThrows<ArgumentException> ());

       Specify (x => x.IsDerivedFrom<Base> ()).
          Case ("simple derived", _ => _
              .GivenSubject ("Derived", x => typeof (Derived))
              .It ("should be derived", x => x.Result.Should ().BeTrue ())).
          Case ("complex derived", _ => _
              .GivenSubject ("DerivedDerived", x => typeof (DerivedDerived))
              .It ("should be derived", x => x.Result.Should ().BeTrue ()))
          .Case ("not derived", _ => _
              .GivenSubject ("NotDerived", x => typeof (NotDerived))
              .It ("should not be derived", x => x.Result.Should ().BeFalse ()));


       Specify (x => x.IsCompoundType ()).
          Case ("value type", _ => _
              .GivenSubject ("int", x => typeof (int))
              .It ("should not be compound type", x => x.Result.Should ().BeFalse ())).
          Case ("complex type without default constructor", _ => _
              .GivenSubject ("ComplexTypeWithoutDefaultConstructor", x => typeof (ComplexTypeWithoutDefaultConstructor))
              .It ("should not be compound type", x => x.Result.Should ().BeFalse ()))
          .Case ("complex type with private default constructor", _ => _
              .GivenSubject ("ComplexTypeWithPrivateDefaultConstructor", x => typeof (ComplexTypeWithPrivateDefaultConstructor))
              .It ("should not be compound type", x => x.Result.Should ().BeFalse ()))
          .Case ("complex type with default constructor", _ => _
              .GivenSubject ("ComplexTypeWithDefaultConstructor", x => typeof (ComplexTypeWithDefaultConstructor))
              .It ("should be compound type", x => x.Result.Should ().BeTrue ()));

      Specify (x => x.CanBeInstantiated (IncludeNonPublicConstructor)).
          Case ("value type", _ => _
              .GivenSubject ("int", x => typeof (int))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ())).
          Case ("complex type without default constructor", _ => _
              .GivenSubject ("ComplexTypeWithoutDefaultConstructor", x => typeof (ComplexTypeWithoutDefaultConstructor))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ()))
          .Case ("complex type with private default constructor", _ => _
              .GivenSubject ("ComplexTypeWithPrivateDefaultConstructor", x => typeof (ComplexTypeWithPrivateDefaultConstructor))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ()))
          .Case ("complex type with default constructor", _ => _
              .GivenSubject ("ComplexTypeWithDefaultConstructor", x => typeof (ComplexTypeWithDefaultConstructor))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeTrue ()))
          .Case ("complex type with private default constructor including non-public constructors", _ => _
              .GivenSubject ("ComplexTypeWithPrivateDefaultConstructor", x => typeof (ComplexTypeWithPrivateDefaultConstructor))
              .Given ("Include non-public constructors", x => IncludeNonPublicConstructor = true)
              .It ("should be instatiantiable", x => x.Result.Should ().BeTrue ()));


      Specify (x => TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<SimpleDTO, string> (y => y.Name))
          .Case ("Get Property Info works on properties", _ => _
              .GivenSubject("SimpleDTO", x=>typeof(SimpleDTO))
              .It ("PropertyInfo matches", x => x.Result.Should ().BeSameAs (typeof (SimpleDTO).GetProperty ("Name"))));

      Specify (x => TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<SimpleDTO, string> (y => y.SomeField))
          .Case ("Get Property Info throws on methods", _ => _
              .GivenSubject ("SimpleDTO", x => typeof (SimpleDTO))
              .ItThrows<ArgumentException> (x=>"Expression 'y => y.SomeField' refers to a field, not a property."));

      Specify (x => TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<SimpleDTO, string> (y => y.GetSomething ()))
          .Case ("Get Property Info throws on fields", _ => _
              .GivenSubject ("SimpleDTO", x => typeof (SimpleDTO))
              .ItThrows<ArgumentException> (x=>"Expression 'y => y.GetSomething()' refers to a method, not a property."));

      Specify (x => TestDataGeneration.Extensions.TypeExtensions.GetPropertyInfo<SimpleDTO, SimpleDTO> (y => y))
          .Case ("Get Property Info throws on classes", _ => _
              .GivenSubject ("SimpleDTO", x => typeof (SimpleDTO))
              .ItThrows<ArgumentException> (x=> "Expression 'y => y' refers to a method, not a property."));
    }

    class SimpleDTO
    {
      public string Name { get; set; }

      public string SomeField;

      public string GetSomething()
      {
        return "";
      }
    }

    class Base
    {

    }

    class Derived : Base
    {

    }

    class DerivedDerived : Derived
    {

    }

    class NotDerived
    {

    }

    class ComplexTypeWithoutDefaultConstructor
    {
      public ComplexTypeWithoutDefaultConstructor (int something)
      {

      }
    }

    class ComplexTypeWithPrivateDefaultConstructor
    {
      ComplexTypeWithPrivateDefaultConstructor ()
      {

      }
    }

    class ComplexTypeWithDefaultConstructor
    {
      public ComplexTypeWithDefaultConstructor ()
      {

      }
    }
  }
}