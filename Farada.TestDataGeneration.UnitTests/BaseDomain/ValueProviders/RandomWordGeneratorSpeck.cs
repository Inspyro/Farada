using System;
using Farada.TestDataGeneration.BaseDomain.ValueProviders;
using SpecK;
using SpecK.Extension.FakeItEasy;
using SpecK.Specifications;

namespace Farada.TestDataGeneration.UnitTests.BaseDomain.ValueProviders
{
  [Subject(typeof(RandomWordGenerator))]
  public class RandomWordGeneratorSpeck:Specs<RandomWordGenerator>
  {
    [Faked] RandomSyllabileGenerator RandomSyllabileGenerator;

    protected override RandomWordGenerator CreateSubject ()
    {
      return new RandomWordGenerator (RandomSyllabileGenerator);
    }

    [Group]
    public void ConstructorSpeck()
    {
      Specify (x => new RandomWordGenerator (RandomSyllabileGenerator, 5, 3).ToString())
          .Elaborate ("Constructor throws on wrong usage", _ => _
              .ItThrows (typeof (ArgumentOutOfRangeException)));
    }
  }
}
