using System;
using System.Collections.Generic;
using System.Text;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using TestFx.Specifications;
using TestFx.Specifications.InferredApi;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.ValueProviders
{
  public class RandomSyllabileGeneratorSpeck
  {
    [Subject (typeof (RandomSyllabileGenerator), "Constructor")]
    public class ConstructorSpecK : SpecK
    {
      ValueProviderContext<string> Context;

      public ConstructorSpecK ()
      {
        Specify (x => new RandomSyllabileGenerator (5, 3).ToString ())
            .Case ("Constructor throws on wrong usage", _ => _
                .ItThrows (typeof (ArgumentOutOfRangeException)));
      }
    }

    [Subject (typeof (RandomSyllabileGenerator), "Fill")]
    public class FillSpecK : SpecK<RandomSyllabileGenerator>
    {
      ValueProviderContext<string> Context;
      static StringBuilder StringBuilder = new StringBuilder ();

      public FillSpecK ()
      {
        Specify (x => x.Fill (Context, StringBuilder))
            .Case ("fill fills stringbuilder based on context", _ => _
                .Given ("Seeded Context", SeededContext (5))
                .It ("fills stringbuilder with correct syllabile", x => StringBuilder.ToString ().Should ().Be ("her")));
      }

      public override RandomSyllabileGenerator CreateSubject ()
      {
        return new RandomSyllabileGenerator ();
      }

      Arrangement<RandomSyllabileGenerator, Dummy, object> SeededContext (int seed)
      {
        return x =>
        {
          var customValueProviderObjectContext = new CustomValueProviderObjectContext (new Random (seed));
          var customValueProviderContext = new CustomValueProviderContext (customValueProviderObjectContext);

          Context = customValueProviderContext;
        };
      }
    }


    class CustomValueProviderObjectContext : ValueProviderObjectContext
    {
      internal CustomValueProviderObjectContext (Random random)
          : this (new DummyTestDataGenerator (random), () => new object (), typeof (Dummy), new DummyFastProperty ())
      {
      }

      CustomValueProviderObjectContext (
          ITestDataGenerator testDataGenerator,
          Func<object> getPreviousValue,
          Type targetValueType,
          IFastPropertyInfo fastPropertyInfo)
          : base (testDataGenerator, getPreviousValue, targetValueType, fastPropertyInfo)
      {
      }
    }

    class DummyFastProperty : IFastPropertyInfo
    {
      public T GetCustomAttribute<T> () where T : Attribute
      {
        throw new NotImplementedException ();
      }

      public IEnumerable<Type> Attributes { get { throw new NotImplementedException (); } }

      public bool IsDefined (Type type)
      {
        throw new NotImplementedException ();
      }

      public object GetValue (object instance)
      {
        throw new NotImplementedException ();
      }

      public void SetValue (object instance, object value)
      {
        throw new NotImplementedException ();
      }

      public string Name { get { throw new NotImplementedException (); } }

      public Type Type { get { throw new NotImplementedException (); } }
    }

    class DummyTestDataGenerator : ITestDataGenerator
    {
      public DummyTestDataGenerator (Random random)
      {
        Random = random;
      }

      public TCompoundValue Create<TCompoundValue> (int maxRecursionDepth = 2, IFastPropertyInfo propertyInfo = null)
      {
        throw new NotSupportedException ();
      }

      public IReadOnlyList<TCompoundValue> CreateMany<TCompoundValue> (
          int numberOfObjects,
          int maxRecursionDepth = 2,
          IFastPropertyInfo propertyInfo = null)
      {
        throw new NotSupportedException ();
      }

      public Random Random { get; private set; }
    }

    class CustomValueProviderContext : ValueProviderContext<string>
    {
      public CustomValueProviderContext (ValueProviderObjectContext objectContext)
          : base (objectContext)
      {
      }
    }
  }
}