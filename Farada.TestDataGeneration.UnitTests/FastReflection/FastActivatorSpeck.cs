using System;
using Farada.TestDataGeneration.FastReflection;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.UnitTests.FastReflection
{
  [Subject (typeof (FastActivator), "GetFactory")]
  public class FastActivatorSpeck:Spec
  {
    Type TypeToCreate;

    public FastActivatorSpeck ()
    {
      Specify (x => FastActivator.GetFactory (TypeToCreate, new IFastArgumentInfo[0]))
          .Case ("creates something", _ => _
              .Given ("object type", x => TypeToCreate = typeof (object))
              .It ("creates an valid object function", x => x.Result.Should ().BeOfType (typeof (Func<object[], object>)))
              .It ("creates a function that returns not null", x => x.Result.Should ().NotBeNull ())
              .It ("creates a new object every time", x => x.Result(new object[0]).Should ().NotBeSameAs (x.Result(new object[0]))))
          .Case ("throws for value type", _ => _
              .Given ("value type", x => TypeToCreate = typeof (int))
              .ItThrows (typeof(NotSupportedException), x=> "Primitive types are not supported. Please configure a value provider for type System.Int32."))
          .Case ("throws for type with no default constructor", _ => _
              .Given ("ClassWithNoDefaultConstructor", x => TypeToCreate = typeof (ClassWithNoDefaultConstructor))
              .ItThrows (typeof(NotSupportedException), x=> "No valid ctor found on 'Farada.TestDataGeneration.UnitTests.FastReflection.ClassWithNoDefaultConstructor': Classes with non-public constructors and abstract classes are not supported."))
          .Case ("throws for type with private constructor", _ => _
              .Given ("ClassWithPrivateConstructor", x => TypeToCreate = typeof (ClassWithPrivateConstructor))
              .ItThrows (typeof(NotSupportedException), x=> "No valid ctor found on 'Farada.TestDataGeneration.UnitTests.FastReflection.ClassWithPrivateConstructor': Classes with non-public constructors and abstract classes are not supported."))
          .Case ("throws for type with internal constructor", _ => _
              .Given ("ClassWithInternalConstructor", x => TypeToCreate = typeof (ClassWithInternalConstructor))
              .ItThrows (typeof(NotSupportedException), x=> "No valid ctor found on 'Farada.TestDataGeneration.UnitTests.FastReflection.ClassWithInternalConstructor': Classes with non-public constructors and abstract classes are not supported."));
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