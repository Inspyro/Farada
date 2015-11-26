using System;
using Farada.TestDataGeneration.Extensions;
using FluentAssertions;
using TestFx.SpecK;

namespace Farada.TestDataGeneration.UnitTests.Extensions
{
  [Subject (typeof (RandomExtensions), "Next")]
  public class RandomExtensionsSpecK :Spec<Random>
  {
    public RandomExtensionsSpecK ()
    {
      Specify (x => x.Next (byte.MinValue, byte.MaxValue))
          .Case ("returns number in byte range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in byte range", x => x.Result.Should ().BeInRange (byte.MinValue, byte.MaxValue)));

      Specify (x => x.Next (decimal.MinValue, decimal.MaxValue))
          .Case ("returns number in decimal range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in decimal range", x => x.Result.Should ().BeInRange (decimal.MinValue, decimal.MaxValue)));

      Specify (x => x.Next (double.MinValue, double.MaxValue))
          .Case ("returns number in double range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in double range", x => x.Result.Should ().BeInRange (double.MinValue, double.MaxValue)));

      Specify (x => x.Next (float.MinValue, float.MaxValue))
          .Case ("returns number in float range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in float range", x => x.Result.Should ().BeInRange (float.MinValue, float.MaxValue)));

      Specify (x => x.Next (long.MinValue, long.MaxValue))
          .Case ("returns number in long range", _ => _
              .Given (DefaultRandomContext ())
              .It ("result is in long range", x => x.Result.Should ().BeInRange (long.MinValue, long.MaxValue)));

      Specify (x => x.Next(uint.MinValue, uint.MaxValue))
          .Case ("returns number in uint range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in uint range", x => x.Result.Should ().BeInRange (uint.MinValue, uint.MaxValue)));

      Specify (x => x.Next(ulong.MinValue, ulong.MaxValue))
          .Case ("returns number in ulong range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in ulong range", x => x.Result.Should ().BeInRange (ulong.MinValue, ulong.MaxValue)));

      Specify (x => x.Next(short.MinValue, short.MaxValue))
          .Case ("returns number in short range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in short range", x => x.Result.Should ().BeInRange (short.MinValue, short.MaxValue)));

      Specify (x => x.Next(ushort.MinValue, ushort.MaxValue))
          .Case ("returns number in ushort range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in ushort range", x => x.Result.Should ().BeInRange (ushort.MinValue, ushort.MaxValue)));

      Specify (x => x.Next (sbyte.MinValue, sbyte.MaxValue))
          .Case ("returns number in sbyte range", _ => _
            .Given(DefaultRandomContext())
              .It ("result is in sbyte range", x => x.Result.Should ().BeInRange (sbyte.MinValue, sbyte.MaxValue)));

      ////NextSeed..
      Specify (x => x.Next (long.MinValue, long.MaxValue))
          .Case ("returns number in long range according to seed", _ => _
              .Given (SeededRandomContext (0))
              .It ("result is always 10", x => x.Result.Should ().Be(2086725849749066753)));
    }
    
    static Context<Random> DefaultRandomContext ()
    {
      return c => c.GivenSubject ("default random", x => new Random ());
    }

    static Context<Random> SeededRandomContext (int seed)
    {
      return c => c.GivenSubject ("random with seed " + seed, x => new Random (seed));
    }
  }
}