using System;
using System.Collections.Generic;
using System.Text;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using Farada.TestDataGeneration.CompoundValueProviders;
using Farada.TestDataGeneration.FastReflection;
using Farada.TestDataGeneration.ValueProviders;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;
using SpecK.Specifications.Extensions;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.ValueProviders
{
  [Subject(typeof(RandomSyllabileGenerator))]
  public class RandomSyllabileGeneratorSpeck:Specs<RandomSyllabileGenerator>
  {
    ValueProviderContext<string> Context;
    StringBuilder StringBuilder=new StringBuilder();

    protected override RandomSyllabileGenerator CreateSubject ()
    {
      return new RandomSyllabileGenerator ();
    }

    Context SeededContext (int seed)
    {
      return c => c.Given (x =>
      {
        var customValueProviderObjectContext = new CustomValueProviderObjectContext (new Random (seed));
        var customValueProviderContext = new CustomValueProviderContext (customValueProviderObjectContext);

        Context = customValueProviderContext;
      });
    }

    [Group]
    public void FillingSpecks()
    {
      Specify (x => new RandomSyllabileGenerator (5, 3).ToString())
          .Elaborate ("Constructor throws on wrong usage", _ => _
              .ItThrows (typeof (ArgumentOutOfRangeException)));

      Specify (x => x.Fill (Context, StringBuilder))
          .Elaborate ("fill fills stringbuilder based on context", _ => _
              .Given (SeededContext (5))
              .It ("fills stringbuilder with correct syllabile", x => StringBuilder.ToString ().Should ().Be ("her")));
    }
  }

  class CustomValueProviderObjectContext : ValueProviderObjectContext
  {
    internal CustomValueProviderObjectContext(Random random):this(new DummyTestDataGenerator(random), ()=>new object(),null,null)
    {

    }

    CustomValueProviderObjectContext (ITestDataGenerator testDataGenerator, Func<object> getPreviousValue, Type targetValueType, IFastPropertyInfo fastPropertyInfo)
        : base (testDataGenerator, getPreviousValue, targetValueType, fastPropertyInfo)
    {
    }
  }

    class DummyTestDataGenerator : ITestDataGenerator
    {
        public DummyTestDataGenerator (Random random)
        {
            Random = random;
        }

        public TCompoundValue Create<TCompoundValue> (int maxRecursionDepth = 2, IFastPropertyInfo propertyInfo = null)
        {
            return null;
        }

        public IReadOnlyList<TCompoundValue> CreateMany<TCompoundValue> (int numberOfObjects, int maxRecursionDepth = 2, IFastPropertyInfo propertyInfo = null)
        {
            return null;
        }

        public Random Random { get; private set; }
    }

    class CustomValueProviderContext : ValueProviderContext<string>
  {
    public CustomValueProviderContext (ValueProviderObjectContext objectContext):base(objectContext)
    {
    }
  }
}
