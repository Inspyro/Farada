using System;
using Farada.Core.FastReflection;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;

namespace Farada.Core.UnitTests.FastReflection
{
  [Subject (typeof (FastActivator))]
  public class FastActivatorSpeck:Specs
  {
    Type TypeToCreate;

    [Group]
    void GetFactory ()
    {
      Specify (x => FastActivator.GetFactory (TypeToCreate))
          .Elaborate ("creates something", _ => _
              .Given ("object type", x => TypeToCreate = typeof (object))
              .It ("creates an valid object function", x => x.Result.Should ().BeOfType (typeof (Func<object>)))
              .It ("creates a fucntion that returns not null", x => x.Result ().Should ().NotBeNull ())
              .It ("creates a new object every time", x => x.Result ().Should ().NotBeSameAs (x.Result ())))
          .Elaborate ("throws for value type", _ => _
              .Given ("value type", x => TypeToCreate = typeof (int))
              .ItThrows (typeof (ArgumentException), "Value types cannot be instantiated"))
          .Elaborate ("throws for type with no default constructor", _ => _
              .Given ("ClassWithNoDefaultConstructor", x => TypeToCreate = typeof (ClassWithNoDefaultConstructor))
              .ItThrows (typeof (NotSupportedException), "Classes without default constructors are not supported"))
          .Elaborate ("throws for type with private constructor", _ => _
              .Given ("ClassWithPrivateConstructor", x => TypeToCreate = typeof (ClassWithPrivateConstructor))
              .ItThrows (typeof (NotSupportedException), "Classes with non-public constructors are not supported"))
          .Elaborate ("throws for type with internal constructor", _ => _
              .Given ("ClassWithInternalConstructor", x => TypeToCreate = typeof (ClassWithInternalConstructor))
              .ItThrows (typeof (NotSupportedException), "Classes with non-public constructors are not supported"));
    }
  }

  class ClassWithInternalConstructor
  {
    internal ClassWithInternalConstructor()
    {

    }
  }

  class ClassWithPrivateConstructor
  {
    private ClassWithPrivateConstructor()
    {

    }
  }

  class ClassWithNoDefaultConstructor
  {
    public ClassWithNoDefaultConstructor(int something)
    {

    }
  }
}