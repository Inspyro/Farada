using System;

namespace Farada.Evolution.Utilities
{
  public static class MathUtility
  {
    public static float Percentage(float value1, float value2)
    {
      return Math.Max(0, Math.Min(1f, value1 / value2));
    }

    public static float Lerp(float value1, float value2, float percentage)
    {
      if (value1 > value2)
      {
        var temp = value1;
        value1 = value2;
        value2 = temp;

        percentage = 1 - percentage;
      }

      var difference = value2 - value1;
      return value1 + difference * percentage;
    }
  }
}
