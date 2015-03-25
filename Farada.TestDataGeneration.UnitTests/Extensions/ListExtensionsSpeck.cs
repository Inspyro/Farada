using System;
using System.Collections.Generic;
using Farada.TestDataGeneration.Extensions;
using FluentAssertions;
using TestFx.Specifications;

namespace Farada.TestDataGeneration.UnitTests.Extensions
{
  [Subject (typeof (ListExtensions), "TODO")]
  public class ListExtensionsSpeck:SpecK<List<int>>
  {
    Random Random;

    public ListExtensionsSpeck ()
    {
      Specify (x => x.Randomize (Random))
          .Case ("randomizes list", _ => _
            .GivenSubject("some list ", x => new List<int> { 1, 2, 3 })
              .Given (SeededRandomContext (1))
              .It ("result is unordered list", x => x.Subject.Should ().Equal (2, 3, 1)));

      Specify (x => x.Slice (1))
          .Case ("removes first element", _ => _
              .GivenSubject ("some list", x => new List<int> { 1, 2, 3 })
              .Given (SeededRandomContext (1))
              .It ("result is list without first element", x => x.Result.Should ().Equal (2, 3)))
          .Case ("for empty list", _ => _
              .GivenSubject ("empty list", x =>  new List<int> ())
              .Given (SeededRandomContext (1))
              .ItThrows <ArgumentException>());
    }

    Context SeededRandomContext (int seed)
    {
      return c => c.Given ("random with seed " + seed, x => Random = new Random (seed));
    }
  }
}