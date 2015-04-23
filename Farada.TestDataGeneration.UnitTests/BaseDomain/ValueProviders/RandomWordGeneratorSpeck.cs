using System;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using TestFx.FakeItEasy;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.ValueProviders
{
  [Subject(typeof(RandomWordGenerator),"Constructor")]
  public class RandomWordGeneratorSpeck:SpecK
  {
    [Faked] RandomSyllabileGenerator RandomSyllabileGenerator;

    public RandomWordGeneratorSpeck ()
    {
       Specify (x => new RandomWordGenerator (RandomSyllabileGenerator, 5, 3).ToString())
          .Case ("Constructor throws on wrong usage", _ => _
              .ItThrows (typeof(ArgumentOutOfRangeException)));
    }
  }
}
