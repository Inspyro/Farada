using System;

namespace Rubicon.RegisterNova.Infrastructure.Utilities
{
  public static class RandomExtensions
  {
    public static float Next(this Random random, float minValue=float.MinValue, float maxValue=float.MaxValue)
    {
      return Next(random, minValue, maxValue, (f, f1) => f + f1, (f, f1) => f - f1, (f, d) => (float) (f * d));
    }

    public static long Next(this Random random, long minValue=long.MinValue, long maxValue=long.MaxValue)
    {
      return Next(random, minValue, maxValue, (f, f1) => f + f1, (f, f1) => f - f1, (f, d) => (long) (f * d));
    }

     public static double Next(this Random random, double minValue=double.MinValue, double maxValue=double.MaxValue)
    {
      return Next(random, minValue, maxValue, (f, f1) => f + f1, (f, f1) => f - f1, (f, d) => (double) (f * d));
    }

    public static ulong Next(this Random random, ulong minValue=ulong.MinValue, ulong maxValue=ulong.MaxValue)
    {
      return Next(random, minValue, maxValue, (f, f1) => f + f1, (f, f1) => f - f1, (f, d) => (ulong) (f * d));
    }

    public static ushort NextUShort(this Random random, ushort minValue=ushort.MinValue, ushort maxValue=ushort.MaxValue)
    {
      return Next(random, minValue, maxValue, (f, f1) => (ushort) (f + f1), (f, f1) => (ushort) (f - f1), (f, d) => (ushort) (f * d));
    }

    public static short NextShort(this Random random, short minValue=short.MinValue, short maxValue=short.MaxValue)
    {
      return Next(random, minValue, maxValue, (f, f1) => (short) (f + f1), (f, f1) => (short) (f - f1), (f, d) => (short) (f * d));
    }

    public static uint Next(this Random random, uint minValue=uint.MinValue, uint maxValue=uint.MaxValue)
    {
      return Next(random, minValue, maxValue, (f, f1) => f + f1, (f, f1) => f - f1, (f, d) => (uint) (f * d));
    }

    public static decimal Next(this Random random, decimal minValue=decimal.MinValue, decimal maxValue=decimal.MaxValue)
    {
      return Next(random, minValue, maxValue, (f, f1) => f + f1, (f, f1) => f - f1, (f, d) => f * (decimal) d);
    }

    public static T Next<T>(this Random random, T minValue, T maxValue, Func<T,T, T> addFunc, Func<T,T, T> substractFunc, Func<T, double, T> multiplyFunc)
    {
      var difference = substractFunc(maxValue, minValue);
      var randomAdd = multiplyFunc(difference, random.NextDouble());

      return addFunc(minValue, randomAdd);
    }
  }
}
