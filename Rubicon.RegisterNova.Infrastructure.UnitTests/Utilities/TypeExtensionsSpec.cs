using System;
using Rubicon.RegisterNova.Infrastructure.Utilities;
using SpecK;
using FluentAssertions;
using SpecK.Specifications;

namespace Rubicon.RegisterNova.Infrastructure.UnitTests.Utilities
{
  [Subject (typeof (Infrastructure.Utilities.TypeExtensions))]
  public class TypeExtensionsSpeck:Specs<Type>
  {
    bool IncludeNonPublicConstructor;

    [Group]
    void IsDerived ()
    {
      Specify (x => x.IsDerivedFrom<Base> ()).
          Elaborate ("simple derived", _ => _
              .GivenSubject ("Derived", x => typeof (Derived))
              .It ("should be derived", x => x.Result.Should ().BeTrue ())).
          Elaborate ("complex derived", _ => _
              .GivenSubject ("DerivedDerived", x => typeof (DerivedDerived))
              .It ("should be derived", x => x.Result.Should ().BeTrue ()))
          .Elaborate ("not derived", _ => _
              .GivenSubject ("NotDerived", x => typeof (NotDerived))
              .It ("should not be derived", x => x.Result.Should ().BeFalse ()));
    }

    [Group]
    void IsCompoundType ()
    {
      Specify (x => x.IsCompoundType ()).
          Elaborate ("value type", _ => _
              .GivenSubject ("int", x => typeof (int))
              .It ("should not be compound type", x => x.Result.Should ().BeFalse ())).
          Elaborate ("complex type without default constructor", _ => _
              .GivenSubject ("ComplexTypeWithoutDefaultConstructor", x => typeof (ComplexTypeWithoutDefaultConstructor))
              .It ("should not be compound type", x => x.Result.Should ().BeFalse ()))
          .Elaborate ("complex type with private default constructor", _ => _
              .GivenSubject ("ComplexTypeWithPrivateDefaultConstructor", x => typeof (ComplexTypeWithPrivateDefaultConstructor))
              .It ("should not be compound type", x => x.Result.Should ().BeFalse ()))
          .Elaborate ("complex type with default constructor", _ => _
              .GivenSubject ("ComplexTypeWithDefaultConstructor", x => typeof (ComplexTypeWithDefaultConstructor))
              .It ("should be compound type", x => x.Result.Should ().BeTrue ()));
    }

    [Group]
    void CanBeInstantiated()
    {
      Specify (x => x.CanBeInstantiated (IncludeNonPublicConstructor)).
          Elaborate ("value type", _ => _
              .GivenSubject ("int", x => typeof (int))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ())).
          Elaborate ("complex type without default constructor", _ => _
              .GivenSubject ("ComplexTypeWithoutDefaultConstructor", x => typeof (ComplexTypeWithoutDefaultConstructor))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ()))
          .Elaborate ("complex type with private default constructor", _ => _
              .GivenSubject ("ComplexTypeWithPrivateDefaultConstructor", x => typeof (ComplexTypeWithPrivateDefaultConstructor))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeFalse ()))
          .Elaborate ("complex type with default constructor", _ => _
              .GivenSubject ("ComplexTypeWithDefaultConstructor", x => typeof (ComplexTypeWithDefaultConstructor))
              .It ("should be not instatiantiable", x => x.Result.Should ().BeTrue ()))
          .Elaborate ("complex type with private default constructor including non-public constructors", _ => _
              .GivenSubject ("ComplexTypeWithPrivateDefaultConstructor", x => typeof (ComplexTypeWithPrivateDefaultConstructor))
              .Given ("Include non-public constructors", x => IncludeNonPublicConstructor = true)
              .It ("should be instatiantiable", x => x.Result.Should ().BeTrue ()));
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
  }

  class ComplexTypeWithoutDefaultConstructor
  {
    public ComplexTypeWithoutDefaultConstructor(int something)
    {

    }
  }

  class ComplexTypeWithPrivateDefaultConstructor
  {
    private ComplexTypeWithPrivateDefaultConstructor()
    {

    }
  }

  class ComplexTypeWithDefaultConstructor
  {
    public ComplexTypeWithDefaultConstructor()
    {

    }
  }
}