using System;
using Farada.Evolution.Utilities;

namespace Farada.Evolution.Utilities
{
  public static class LerpUtility
  {
    public static float LerpFromLowToHigh(int highValueCount, int currentCount, float lowPercentage, float highPercentage)
    {
      var percentage = MathUtility.Percentage(currentCount, highValueCount);
      return MathUtility.Lerp(lowPercentage, highPercentage, percentage);
    }
  }
}
