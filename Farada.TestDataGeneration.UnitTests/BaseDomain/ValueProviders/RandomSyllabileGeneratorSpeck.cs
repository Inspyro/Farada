using System;
using System.Collections.Generic;
using System.Text;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders.Keys;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using JetBrains.Annotations;
using TestFx.SpecK;
using TestFx.SpecK.InferredApi;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.ValueProviders
{
  public class RandomSyllabileGeneratorSpeck
  {
    [Subject (typeof (RandomSyllabileGenerator), "Constructor")]
    public class ConstructorSpecK : Spec
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
    public class FillSpecK : Spec<RandomSyllabileGenerator>
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

      Arrangement<RandomSyllabileGenerator, Dummy, object, object> SeededContext (int seed)
      {
        return x =>
        {
          var customValueProviderObjectContext = new CustomValueProviderObjectContext (new DefaultRandom (seed));
          var customValueProviderContext = new CustomValueProviderContext (customValueProviderObjectContext);

          Context = customValueProviderContext;
        };
      }
    }


    class CustomValueProviderObjectContext : ValueProviderObjectContext
    {
      internal CustomValueProviderObjectContext (IRandom random)
          : base (new DummyTestDataGenerator (random), (x) => new object (), typeof (Dummy), new DummyAdvancedContext(), new DummyFastProperty ())
      {
      }
    }

    class DummyFastProperty : IFastMemberWithValues
    {
      public T GetCustomAttribute<T> () where T : Attribute
      {
        throw new NotImplementedException ();
      }

      public IEnumerable<T> GetCustomAttributes<T> () where T : Attribute
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

      public void SetValue (object instance, [CanBeNull] object value)
      {
        throw new NotImplementedException ();
      }

      public string Name { get { throw new NotImplementedException (); } }

      public Type Type { get { throw new NotImplementedException (); } }
    }

    class DummyTestDataGenerator : ITestDataGenerator
    {
      public DummyTestDataGenerator (IRandom random)
      {
        Random = random;
      }

      public TCompoundValue Create<TCompoundValue> (int maxRecursionDepth = 2, IFastMemberWithValues member = null)
      {
        throw new NotSupportedException ();
      }

      public IList<TCompoundValue> CreateMany<TCompoundValue> (
          int numberOfObjects,
          int maxRecursionDepth = 2,
          IFastMemberWithValues member = null)
      {
        throw new NotSupportedException ();
      }

      public object Create (Type type, int maxRecursionDepth = 2, IFastMemberWithValues member = null)
      {
        throw new NotSupportedException();
      }

      public IList<object> CreateMany (Type type, int numberOfObjects, int maxRecursionDepth = 2, IFastMemberWithValues member = null)
      {
        throw new NotSupportedException();
      }

      public IRandom Random { get; }
    }

    class CustomValueProviderContext : ValueProviderContext<string>
    {
      public CustomValueProviderContext (ValueProviderObjectContext objectContext)
          : base (objectContext)
      {
      }
    }
  }

  class DummyAdvancedContext : ValueProviderObjectContext.AdvancedContext
  {
    public DummyAdvancedContext ()
        : base (new DummyKey(), new DummySorter(), new DummyResolver(), new DummyConverter(), new DummyTestDataGenerator())
    {
    }
  }

  class DummyResolver : IMetadataResolver
  {
    public bool NeedsMetadata (IKey memberKey)
    {
      throw new NotImplementedException ();
    }

    public IEnumerable<object> Resolve (IKey memberKey, IList<MetadataObjectContext> metadataContexts)
    {
      throw new NotImplementedException ();
    }
  }

  class DummySorter : IMemberSorter
  {
    public IEnumerable<IFastMemberWithValues> Sort (IEnumerable<IFastMemberWithValues> members, IKey baseKey)
    {
      throw new NotImplementedException ();
    }
  }

  class DummyTestDataGenerator : ITestDataGeneratorAdvanced
  {
    public IList<object> CreateMany (IKey currentKey, IList<object> resolvedMetadatasForKey, int itemCount, int maxRecursionDepth)
    {
      throw new NotImplementedException ();
    }
  }

  class DummyConverter : IParameterConversionService
  {
    public string ToPropertyName (string parameterName)
    {
      throw new NotSupportedException();
    }
  }

  class DummyKey : IKey
  {
    public bool Equals ([CanBeNull] IKey other)
    {
      if (other == null)
        throw new ArgumentNullException (nameof (other));

      throw new NotSupportedException();
    }

    public IKey PreviousKey { get; }

    public IKey CreateKey (IFastMemberWithValues member)
    {
      throw new NotSupportedException();
    }

    public Type Type { get; }

    public IFastMemberWithValues Member { get; }

    public int RecursionDepth  { get; }

  public IKey ChangeMemberType (Type newMemberType)
    {
      throw new NotSupportedException();
    }
  }
}