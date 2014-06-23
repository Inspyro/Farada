using System;
using System.Linq;
using Rubicon.RegisterNova.Infrastructure.TestData;
using Rubicon.RegisterNova.Infrastructure.TestData.Parallelization;

namespace Rubicon.RegisterNova.PerformanceTests
{
  internal class Program
  {
    private static void Main ()
    {
      var basicDomain = new DomainConfiguration();
      var valueProvider = TestDataGeneratorFactory.CreateCompoundValueProvider(basicDomain);

      const int count = 1000000; //8 million

      var start = DateTime.Now;
      var listOfUniverses = Parallelization.DistributeParallel(chunkCount => valueProvider.CreateMany<Universe>(chunkCount), count).ToList();
      //var listOfUniverses = valueProvider.CreateMany<Universe>(count).ToList();

      Console.WriteLine(
          "Took {0} s to generate {1} universes",
          (DateTime.Now - start).TotalSeconds,
          listOfUniverses.Count);
    }

    internal class Universe
    {
      public string Name { get; set; }
      public float SizeInLightYears { get; set; }
      public int PeopleCount { get; set; }

      public StarSystem StarSystem { get; set; } //TODO declare as List as soon as available
    }

    internal class StarSystem
    {
      public string Name { get; set; }
      public float SizeInLightYears { get; set; }
      public int PeopleCount { get; set; }

      public Planet Planet { get; set; } //TODO declare as List as soon as available
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
