using System;
using System.Linq;
using Farada.Core.Extensions;
using FluentAssertions;
using SpecK;
using SpecK.Specifications;

namespace Farada.Core.UnitTests.Extensions
{
  [Subject (typeof (EnumerableExtensions))]
  public class EnumerableExtensionsSpeck:Specs
  {
    Type TypeToCreate;
    int NumberOfObjects;
    object[] ObjectArray;

    [Group]
    void Repeat ()
    {
      Specify (x => EnumerableExtensions.Repeat (() => new object (), NumberOfObjects).ToList ())
          .Elaborate ("for 10 objects", _ => _
              .Given ("10 objects", x => NumberOfObjects = 10)
              .It ("creates enumerable with exactly 10 objects", x => x.Result.Count.Should ().Be (10))
              .It ("creates enumerable with 10 different objects", x => x.Result.All (o => x.Result.Count (a => a == o) == 1).Should ().BeTrue ()))
          .Elaborate ("for 0 objects", _ => _
              .Given ("0 objects", x => NumberOfObjects = 0)
              .It ("creates empty enumerable", x => x.Result.Count.Should ().Be (0)))
              .Elaborate("for invalid number of objects", _=>_
              .Given("-1 objects",x=>NumberOfObjects=-1)
              .It("creates empty enumerable", x=>x.Result.Count.Should().Be(0)));
    }

    [Group]
    void CastOrDefault ()
    {
      Specify (x => ObjectArray.CastOrDefault<CustomType> ().ToList ())
          .Elaborate ("for normal array", _ => _
              .Given ("CustomType array", x => ObjectArray = new object[] { new CustomType (1), new CustomType (2), new CustomType (3) })
              .It ("casts each element",
                  x => x.Result.Should ().BeEquivalentTo (new CustomType (1), new CustomType (2), new CustomType (3))))
          .Elaborate ("for sparse array", _ => _
              .Given ("CustomType sparse array", x => ObjectArray = new object[] { new CustomType (1), null, new CustomType (3) })
              .It ("casts each element and sparse elements to default",
                  x => x.Result.Should ().BeEquivalentTo (new CustomType (1), null, new CustomType (3))));

      Specify (x => ObjectArray.CastOrDefault<int> ().ToList ())
          .Elaborate ("for sparse int array", _ => _
              .Given ("int sparse array", x => ObjectArray = new object[] { 1, null, 3 })
              .It ("casts each element and sparse elements to default",
                  x => x.Result.Should ().BeEquivalentTo (1, 0, 3)));
    }

    class CustomType:IEquatable<CustomType>
    {
      readonly int _value;

      public CustomType (int value)
      {
        _value = value;
      }

      public bool Equals (CustomType other)
      {
        if (!EqualityUtility.ClassEquals (this, other))
          return false;

        return _value == other._value;
      }

      public override bool Equals (object obj)
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