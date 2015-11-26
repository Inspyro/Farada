using System;

namespace Farada.TestDataGeneration.ValueProviders
{
  /// <summary>
  /// Interface for <see cref="Random"/>.
  /// </summary>
  public interface IRandom
  {
    int Next();

    int Next(int maxValue);

    int Next (int minValue, int maxValue);
    double NextDouble ();
    void NextBytes (byte[] randomBytes);
  }

  public class DefaultRandom : Random, IRandom
  {
    public DefaultRandom()
    {
    }

    public DefaultRandom (int seed)
      :base(seed)
    {
    }
  }
}