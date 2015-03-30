﻿using System;
using Farada.TestDataGeneration.FastReflection;
using FluentAssertions;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.UnitTests.FastReflection
{
  [Subject (typeof (FastActivator), "TODO")]
  public class FastActivatorSpeck:SpecK
  {
    Type TypeToCreate;

    public FastActivatorSpeck ()
    {
      //TODO RN-242: Update test to also use a ctor with arguments... and upgrade thrown exceptions as soon as TestFx is working again
      Specify (x => FastActivator.GetFactory (TypeToCreate, null))
          .Case ("creates something", _ => _
              .Given ("object type", x => TypeToCreate = typeof (object))
              .It ("creates an valid object function", x => x.Result.Should ().BeOfType (typeof (Func<object>)))
              .It ("creates a fucntion that returns not null", x => x.Result.Should ().NotBeNull ())
              .It ("creates a new object every time", x => x.Result.Should ().NotBeSameAs (x.Result)))
          .Case ("throws for value type", _ => _
              .Given ("value type", x => TypeToCreate = typeof (int))
              .ItThrows<ArgumentException> (x=>"Value types cannot be instantiated"))
          .Case ("throws for type with no default constructor", _ => _
              .Given ("ClassWithNoDefaultConstructor", x => TypeToCreate = typeof (ClassWithNoDefaultConstructor))
              .ItThrows<NotSupportedException> (x=>"Classes without default constructors are not supported"))
          .Case ("throws for type with private constructor", _ => _
              .Given ("ClassWithPrivateConstructor", x => TypeToCreate = typeof (ClassWithPrivateConstructor))
              .ItThrows <NotSupportedException> (x=>"Classes with non-public constructors are not supported"))
          .Case ("throws for type with internal constructor", _ => _
              .Given ("ClassWithInternalConstructor", x => TypeToCreate = typeof (ClassWithInternalConstructor))
              .ItThrows<NotSupportedException> (x=>"Classes with non-public constructors are not supported"));
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