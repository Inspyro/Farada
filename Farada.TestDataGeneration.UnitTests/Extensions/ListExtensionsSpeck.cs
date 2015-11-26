using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.Extensions;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.UnitTests.Extensions
{
  public class ListExtensionsSpecK
  {
    [Subject (typeof (ListExtensions), "Randomize")]
    public class RandomizeSpecK : Spec<List<int>>
    {
      Random Random;

      public RandomizeSpecK ()
      {
        Specify (x => x.Randomize (Random))
            .Case ("randomizes list", _ => _
                .GivenSubject ("some list ", x => new List<int> { 1, 2, 3 })
                .Given (SeededRandomContext (1))
                .It ("result is unordered list", x => x.Subject.Should ().Equal (2, 3, 1)));
      }

      Context SeededRandomContext (int seed)
      {
        return c => c.Given ("random with seed " + seed, x => Random = new Random (seed));
      }
    }

    [Subject (typeof (ListExtensions), "Slice")]
    public class SliceSpecK : Spec<List<int>>
    {
      Random Random;

      public SliceSpecK ()
      {
        Specify (x => x.Slice (1))
            .Case ("removes first element", _ => _
                .GivenSubject ("some list", x => new List<int> { 1, 2, 3 })
                .It ("result is list without first element", x => x.Result.Should ().Equal (2, 3)))
            .Case ("for empty list", _ => _
                .GivenSubject ("empty list", x => new List<int> ())
                .ItThrows (typeof (ArgumentException)));
      }
    }
  }
}