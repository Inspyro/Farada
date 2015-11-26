using System;
using Farada.TestDataGeneration.ValueProviders;

namespace Farada.TestDataGeneration.Extensions
{
  /// <summary>
  /// This class contains extensions for the <see cref="Random"/> in order to provide random values for other types than int, short or byte
  /// </summary>
  public static class RandomExtensions
  {
    public static float Next (this IRandom random, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
      return Next(
          random,
          Math.Max(float.MinValue / 2 + 1, minValue),
          Math.Min(float.MaxValue / 2 - 1, maxValue),
          (f, f1) => f + f1,
          (f, f1) => f - f1,
          (f, d) => (float) (f * d));
    }

    public static long Next (this IRandom random, long minValue = long.MinValue, long maxValue = long.MaxValue)
    {
      return Next(
          random,
          Math.Max(long.MinValue / 2 + 1, minValue),
          Math.Min(long.MaxValue / 2 - 1, maxValue),
          (f, f1) => f + f1,
          (f, f1) => f - f1,
          (f, d) => (long) (f * d));
    }

    public static double Next (this IRandom random, double minValue = double.MinValue, double maxValue = double.MaxValue)
    {
      return Next(
          random,
          Math.Max(double.MinValue / 2 + 1, minValue),
          Math.Min(double.MaxValue / 2 - 1, maxValue),
          (f, f1) => f + f1,
          (f, f1) => f - f1,
          (f, d) => f * d);
    }

    public static ulong Next (this IRandom random, ulong minValue = ulong.MinValue, ulong maxValue = ulong.MaxValue)
    {
      return Next(
          random,
          Math.Max(ulong.MinValue / 2 + 1, minValue),
          Math.Min(ulong.MaxValue / 2 - 1, maxValue),
          (f, f1) => f + f1,
          (f, f1) => f - f1,
          (f, d) => (ulong) (f * d));
    }

    public static ushort NextUShort (this IRandom random, ushort minValue = ushort.MinValue, ushort maxValue = ushort.MaxValue)
    {
      return Next(
          random,
          (ushort) Math.Max(ushort.MinValue / 2 + 1, (int) minValue),
          (ushort) Math.Min(ushort.MaxValue / 2 - 1, (int) maxValue),
          (f, f1) => (ushort) (f + f1),
          (f, f1) => (ushort) (f - f1),
          (f, d) => (ushort) (f * d));
    }

    public static short NextShort (this IRandom random, short minValue = short.MinValue, short maxValue = short.MaxValue)
    {
      return Next(
          random,
          (short) Math.Max(short.MinValue / 2 + 1, (int) minValue),
          (short) Math.Min(short.MaxValue / 2 - 1, (int) maxValue),
          (f, f1) => (short) (f + f1),
          (f, f1) => (short) (f - f1),
          (f, d) => (short) (f * d));
    }

    public static uint Next (this IRandom random, uint minValue = uint.MinValue, uint maxValue = uint.MaxValue)
    {
      return Next(
          random,
          Math.Max(uint.MinValue / 2 + 1, minValue),
          Math.Min(uint.MaxValue / 2 - 1, maxValue),
          (f, f1) => f + f1,
          (f, f1) => f - f1,
          (f, d) => (uint) (f * d));
    }

    public static decimal Next (this IRandom random, decimal minValue = decimal.MinValue, decimal maxValue = decimal.MaxValue)
    {
      return Next(
          random,
          Math.Max(decimal.MinValue / 2 + 1, minValue),
          Math.Min(decimal.MaxValue / 2 - 1, maxValue),
          (f, f1) => f + f1,
          (f, f1) => f - f1,
          (f, d) => (decimal) ((double) f * d));
    }

    public static T Next<T> (
        this IRandom random,
        T minValue,
        T maxValue,
        Func<T, T, T> addFunc,
        Func<T, T, T> substractFunc,
        Func<T, double, T> multiplyFunc)
    {
      var difference = substractFunc(maxValue, minValue);
      var randomAdd = multiplyFunc(difference, random.NextDouble());

      return addFunc(minValue, randomAdd);
    }
  }
}
