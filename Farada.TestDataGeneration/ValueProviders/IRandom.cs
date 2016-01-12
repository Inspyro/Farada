using System;
using System.Threading;

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

  public class DefaultRandom : IRandom
  {
    private int _seed;
    private readonly ThreadLocal<Random> _random;

    /// <summary>
    /// This instance is thread safe and it guarantees a different seed per thread.
    /// </summary>
    public static readonly DefaultRandom Instance = new DefaultRandom();

    private DefaultRandom ()
    {
      _seed = Environment.TickCount;
      _random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));
    }

    public int Next ()
    {
      return _random.Value.Next();
    }

    /// <summary>
    /// Creates a random that uses the given seed for all threads. 
    /// Use this ctor only for testing purposes, or in single-thread, for multi-threaded Farada use: <see cref="Instance"/>.
    /// </summary>
    /// <param name="seed"></param>
    public DefaultRandom (int seed)
    {
      _random = new ThreadLocal<Random>(() => new Random(seed));
    }

    public int Next (int maxValue)
    {
      return _random.Value.Next (maxValue);
    }

    public int Next (int minValue, int maxValue)
    {
      return _random.Value.Next (minValue, maxValue);
    }

    public double NextDouble ()
    {
      return _random.Value.NextDouble();
    }

    public void NextBytes (byte[] randomBytes)
    {
      _random.Value.NextBytes (randomBytes);
    }
  }
}