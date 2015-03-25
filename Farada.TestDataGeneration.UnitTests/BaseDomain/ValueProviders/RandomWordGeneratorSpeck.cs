using System;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using TestFx.FakeItEasy;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.ValueProviders
{
  [Subject(typeof(RandomWordGenerator),"TODO")]
  public class RandomWordGeneratorSpeck:SpecK<RandomWordGenerator>
  {
    [Faked] RandomSyllabileGenerator RandomSyllabileGenerator;

    public RandomWordGeneratorSpeck ()
    {
       Specify (x => new RandomWordGenerator (RandomSyllabileGenerator, 5, 3).ToString())
          .Case ("Constructor throws on wrong usage", _ => _
              .ItThrows<ArgumentOutOfRangeException> ());
    }

    public override RandomWordGenerator CreateSubject ()
    {
      return new RandomWordGenerator (RandomSyllabileGenerator);
    }
  }
}
