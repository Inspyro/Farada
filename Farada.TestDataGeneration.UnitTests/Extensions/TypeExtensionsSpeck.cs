using System;
using Farada.TestDataGeneration.Extensions;
using FluentAssertions;
using TestFx.SpecK;
using TypeExtensions = Farada.TestDataGeneration.Extensions.TypeExtensions;

namespace Farada.TestDataGeneration.UnitTests.Extensions
{
  public class TypeExtensionsSpecK : Spec
  {
    Type Type;

    [Subject (typeof (TypeExtensions), nameof(String))]
    public class IsNullableTypeSpecK : TypeExtensionsSpecK
    {
      public IsNullableTypeSpecK ()
      {
        Specify (x => Type.IsUnwrappableNullableType ())
            .Case ("simple nullable", _ => _
                .Given ("int?", x => Type = typeof (int?))
                .It ("should be nullable", x => x.Result.Should ().BeTrue ()))
            .Case ("complex nullable", _ => _
                .Given ("Nullable<>", x => Type = typeof (Nullable<>))
                .It ("should be nullable", x => x.Result.Should ().BeFalse ()))
            .Case ("non nullable", _ => _
                .Given ("int", x => Type = typeof (int))
                .It ("should not be nullable", x => x.Result.Should ().BeFalse ()))
            .Case ("nullable class", _ => _
                .Given ("string", x => Type = typeof (string))
                .It ("should not be nullable", x => x.Result.Should ().BeFalse ()));
      }
    }

    [Subject (typeof (TypeExtensions), nameof (TypeExtensions.UnwrapIfNullable))]
    public class UnwrapIfNullableSpecK : TypeExtensionsSpecK
    {
      public UnwrapIfNullableSpecK ()
      {
        Specify (x => Type.UnwrapIfNullable ())
            .Case ("simple nullable", _ => _
                .Given ("int?", x => Type = typeof (int?))
                .It ("should be int", x => x.Result.Should ().Be (typeof (int))))
            .Case ("complex nullable", _ => _
                .Given ("Nullable<>", x => Type = typeof (Nullable<>))
                .It ("should skip non-unwrappable nullable", x => x.Result.Should ().Be (typeof (Nullable<>))))
            .Case ("non nullable", _ => _
                .Given ("int", x => Type = typeof (int))
                .It ("should be int", x => x.Result.Should ().Be (typeof (int))));
      }
    }

    [Subject (typeof (TypeExtensions), nameof (TypeExtensions.IsCompoundType))]
    public class IsCompoundTypeSpecK : TypeExtensionsSpecK
    {
      public IsCompoundTypeSpecK ()
      {
        Specify (x => Type.IsCompoundType ()).
            Case ("value type", _ => _
                .Given ("int", x => Type=typeof (int))
                .It ("should not be compound type", x => x.Result.Should ().BeFalse ())).
            Case ("complex type without default constructor", _ => _
                .Given ("ComplexTypeWithoutDefaultConstructor", x => Type= typeof (ComplexTypeWithoutDefaultConstructor))
                .It ("should not be compound type", x => x.Result.Should ().BeFalse ()))
            .Case ("complex type with private default constructor", _ => _
                .Given ("ComplexTypeWithPrivateDefaultConstructor", x => Type= typeof (ComplexTypeWithPrivateDefaultConstructor))
                .It ("should not be compound type", x => x.Result.Should ().BeFalse ()))
            .Case ("complex type with default constructor", _ => _
                .Given ("ComplexTypeWithDefaultConstructor", x => Type=typeof (ComplexTypeWithDefaultConstructor))
                .It ("should be compound type", x => x.Result.Should ().BeTrue ()));
      }
    }

    [Subject (typeof (TypeExtensions), nameof (TypeExtensions.CanBeInstantiated))]
    public class CanBeInstantiatedSpecK : TypeExtensionsSpecK
    {
      bool IncludeNonPublicConstructor;

      public CanBeInstantiatedSpecK ()
      {
        Specify (x => Type.CanBeInstantiated (IncludeNonPublicConstructor)).
            Case ("value type", _ => _
                .Given ("int", x => Type=typeof (int))
                .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ())).
            Case ("complex type without default constructor", _ => _
                .Given ("ComplexTypeWithoutDefaultConstructor", x => Type=typeof (ComplexTypeWithoutDefaultConstructor))
                .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ()))
            .Case ("complex type with private default constructor", _ => _
                .Given ("ComplexTypeWithPrivateDefaultConstructor", x => Type=typeof (ComplexTypeWithPrivateDefaultConstructor))
                .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ()))
            .Case ("complex type with default constructor", _ => _
                .Given ("ComplexTypeWithDefaultConstructor", x => Type=typeof (ComplexTypeWithDefaultConstructor))
                .It ("should be not instatiantiable", x => x.Result.Should ().BeTrue ()))
            .Case ("complex type with private default constructor including non-public constructors", _ => _
                .Given ("ComplexTypeWithPrivateDefaultConstructor", x => Type=typeof (ComplexTypeWithPrivateDefaultConstructor))
                .Given ("Include non-public constructors", x => IncludeNonPublicConstructor = true)
                .It ("should be instatiantiable", x => x.Result.Should ().BeTrue ()));
      }
    }

    [Subject (typeof (TypeExtensions), nameof (TypeExtensions.GetPropertyInfo))]
    public class GetPropertyInfoSpecK : TypeExtensionsSpecK
    {
      bool IncludeNonPublicConstructor;

      public GetPropertyInfoSpecK ()
      {
        Specify (x => TypeExtensions.GetPropertyInfo<SimpleDTO, string> (y => y.Name))
            .Case ("Get Property Info works on properties", _ => _
                .Given ("SimpleDTO", x => Type=typeof (SimpleDTO))
                .It ("PropertyInfo matches", x => x.Result.Should ().BeSameAs (typeof (SimpleDTO).GetProperty ("Name"))));

        Specify (x => TypeExtensions.GetPropertyInfo<SimpleDTO, string> (y => y.SomeField))
            .Case ("Get Property Info throws on methods", _ => _
                .Given ("SimpleDTO", x => Type=typeof (SimpleDTO))
                .ItThrows (typeof (ArgumentException), x => "Expression 'y => y.SomeField' refers to a field, not a property."));

        Specify (x => TypeExtensions.GetPropertyInfo<SimpleDTO, string> (y => y.GetSomething ()))
            .Case ("Get Property Info throws on fields", _ => _
                .Given ("SimpleDTO", x => Type=typeof (SimpleDTO))
                .ItThrows (typeof (ArgumentException), x => "Expression 'y => y.GetSomething()' refers to a method, not a property."));

        Specify (x => TypeExtensions.GetPropertyInfo<SimpleDTO, SimpleDTO> (y => y))
            .Case ("Get Property Info throws on classes", _ => _
                .Given ("SimpleDTO", x => Type=typeof (SimpleDTO))
                .ItThrows (typeof (ArgumentException), x => "Expression 'y => y' refers to a method, not a property."));
      }
    }

    class SimpleDTO
    {
      public string Name { get; set; }

      public string SomeField;

      public string GetSomething ()
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