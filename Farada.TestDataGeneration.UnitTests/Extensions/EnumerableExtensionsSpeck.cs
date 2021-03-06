﻿using System;
using System.Diagnostics;
using System.Linq;
using Farada.TestDataGeneration.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.UnitTests.Extensions
{
  public class EnumerableExtensionsSpeck:Spec
  {
    Type TypeToCreate;
    int NumberOfObjects;
    object[] ObjectArray;
    int[] IntArray;
    Random Random;

    [Subject (typeof (EnumerableExtensions), "Repeat")]
    public class RepeatSpecK : EnumerableExtensionsSpeck
    {
      public RepeatSpecK ()
      {
        Specify (x => EnumerableExtensions.Repeat (() => new object (), NumberOfObjects).ToList ())
            .Case ("for 10 objects", _ => _
                .Given ("10 objects", x => NumberOfObjects = 10)
                .It ("creates enumerable with exactly 10 objects", x => x.Result.Count.Should ().Be (10))
                .It ("creates enumerable with 10 different objects", x => x.Result.All (o => x.Result.Count (a => a == o) == 1).Should ().BeTrue ()))
            .Case ("for 0 objects", _ => _
                .Given ("0 objects", x => NumberOfObjects = 0)
                .It ("creates empty enumerable", x => x.Result.Count.Should ().Be (0)))
            .Case ("for invalid number of objects", _ => _
                .Given ("-1 objects", x => NumberOfObjects = -1)
                .It ("creates empty enumerable", x => x.Result.Count.Should ().Be (0)));
      }
    }

    [Subject(typeof(EnumerableExtensions), "Shuffle")]
    public class ShuffleSpecK : EnumerableExtensionsSpeck
    {
      public ShuffleSpecK()
      {
        Specify (x => EnumerableExtensions.Shuffle (IntArray, () => Random.Next (int.MinValue, int.MaxValue)).ToList ())
            .Case ("for ordered array", _ => _
                .Given("seeded random", x=>Random=new Random(Seed: 0))
                .Given ("ordered array", x => IntArray = new[] { 0, 1, 2, 3 })
                .It ("shuffles all items", x => x.Result.Should ().Equal (3, 0, 2, 1)));
      }
    }

    [Subject (typeof (EnumerableExtensions), "CastOrDefault")]
    public class CastOrDefaultSpecK : EnumerableExtensionsSpeck
    {
      public CastOrDefaultSpecK ()
      {
        Specify (x => ObjectArray.CastOrDefault<CustomType> ().ToList ())
            .Case ("for normal array", _ => _
                .Given ("CustomType array", x => ObjectArray = new object[] { new CustomType (1), new CustomType (2), new CustomType (3) })
                .It ("casts each element",
                    x => x.Result.Should ().BeEquivalentTo (new CustomType (1), new CustomType (2), new CustomType (3))))
            .Case ("for sparse array", _ => _
                .Given ("CustomType sparse array", x => ObjectArray = new object[] { new CustomType (1), null, new CustomType (3) })
                .It ("casts each element and sparse elements to default",
                    x => x.Result.Should ().BeEquivalentTo (new CustomType (1), null, new CustomType (3))));

        Specify (x => ObjectArray.CastOrDefault<int> ().ToList ())
            .Case ("for sparse int array", _ => _
                .Given ("int sparse array", x => ObjectArray = new object[] { 1, null, 3 })
                .It ("casts each element and sparse elements to default",
                    x => x.Result.Should ().BeEquivalentTo (1, 0, 3)));
      }

      class CustomType : IEquatable<CustomType>
      {
        readonly int _value;

        public CustomType (int value)
        {
          _value = value;
        }

        public bool Equals ([CanBeNull] CustomType other)
        {
          if (!EqualityUtility.ClassEquals (this, other))
            return false;

          Trace.Assert (other != null);
          return _value == other._value;
        }

        public override bool Equals ([CanBeNull] object obj)
        {
          return Equals (obj as CustomType);
        }

        public override int GetHashCode ()
        {
          return _value;
        }
      }
    }
  }
}