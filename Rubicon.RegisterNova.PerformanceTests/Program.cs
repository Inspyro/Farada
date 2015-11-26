using System;
using System.Linq;
using Farada.TestDataGeneration;
using Farada.TestDataGeneration.Parallelization;

namespace Rubicon.RegisterNova.PerformanceTests
{
  internal class Program
  {
    private static void Main ()
    {
      const int count = 1000000; //1 million

      var start = DateTime.Now;
      var listOfUniverses =
          Parallelization.DistributeParallel (chunkCount => TestDataGeneratorFactory.Create().CreateMany<Universe> (chunkCount), count).ToList();

      Console.WriteLine (
          "Took {0}s to generate {1} universes",
          (DateTime.Now - start).TotalSeconds,
          listOfUniverses.Count);

      Console.Read();
    }

    internal class Universe
    {
      public string Name { get; set; }
      public float SizeInLightYears { get; set; }
      public int PeopleCount { get; set; }

      public StarSystem StarSystem { get; set; }
    }

    internal class StarSystem
    {
      public string Name { get; set; }
      public float SizeInLightYears { get; set; }
      public int PeopleCount { get; set; }

      public Planet Planet { get; set; }
    }

    internal class Planet
    {
      public string Name { get; set; }
      public float SizeInKilometers { get; set; }
      public int PeopleCount { get; set; }
      public PlanetColor Color { get; set; }
    }

    internal enum PlanetColor
    {
      Blue,
      Red
    }
  }
}
