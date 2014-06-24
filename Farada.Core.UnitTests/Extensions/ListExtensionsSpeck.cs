using System;
using System.Collections.Generic;
using Farada.Core.Extensions;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;

namespace Farada.Core.UnitTests.Extensions
{
  [Subject (typeof (ListExtensions))]
  public class ListExtensionsSpeck:Specs<List<int>>
  {
    Random Random;

    Context SeededRandomContext (int seed)
    {
      return c => c.Given ("random with seed " + seed, x => Random = new Random (seed));
    }

    [Group]
    void Randomize ()
    {
      Specify (x => x.Randomize (Random))
          .Elaborate ("randomizes list", _ => _
            .GivenSubject("some list ", x => new List<int> { 1, 2, 3 })
              .Given (SeededRandomContext (1))
              .It ("result is unordered list", x => x.Subject.Should ().Equal (2, 3, 1)));
    }

    [Group]
    void Slice()
    {
      Specify (x => x.Slice (1))
          .Elaborate ("removes first element", _ => _
              .GivenSubject ("some list", x => new List<int> { 1, 2, 3 })
              .Given (SeededRandomContext (1))
              .It ("result is list without first element", x => x.Result.Should ().Equal (2, 3)))
          .Elaborate ("for empty list", _ => _
              .GivenSubject ("empty list", x =>  new List<int> ())
              .Given (SeededRandomContext (1))
              .ItThrows (typeof (ArgumentException)));
    }
  }
}