using System;

namespace Farada.TestDataGeneration.IntegrationTests.TestDomain
{
  internal class Universe
  {
    public Galaxy Galaxy1 { get; set; }

    internal class Galaxy
    {
      public StarSystem StarSystem1{ get; set; }

      internal class StarSystem
      {
        public Planet Planet1{ get; set; }
        public Moon Moon1{ get; set; }
        public Star Sun{ get; set; }

        internal class Planet
        {
          public Human President{ get; set; }

          internal class Human
          {
            public string Name{ get; set; }
            public Atom Atom1{ get; set; }

            internal class Atom
            {
              public QuantumParticle Particle1{ get; set; }

              internal class QuantumParticle
              {
                public Universe QuantumUniverse{ get; set; }
              }
            }
          }
        }

        internal class Moon
        {

        }

        internal class Star
        {

        }
      }
    }
  }
}