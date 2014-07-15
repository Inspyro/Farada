using System;
using System.Collections.Generic;
using Farada.Evolution;
using Farada.Evolution.RuleBasedDataGeneration;
using Farada.TestDataGeneration;
using PersonDomain.Sample.Models.PersonDomain;

namespace PersonDomain.Sample.Models
{
    public class PersonDomainFactory
    {
        private readonly GeneratorResult _initialData;
        private readonly RuleBasedDataGenerator _dataGenerator;

        public PersonDomainFactory ()
        {
            _dataGenerator = EvolutionaryDataGeneratorFactory.Create (
                    tdConfig => tdConfig
                            //
                            .For<Gender>().AddProvider (context => (Gender) (context.Random.Next (0, 2))),

                    evConfig => evConfig.AddGlobalRule (new WorldRule())
                            .AddRule (new ProcreationRule()).AddRule (new AgingRule()));

            var initialDataProvider = _dataGenerator.InitialDataProvider;
            initialDataProvider.Add (new Person ("Adam", Gender.Male));
            initialDataProvider.Add (new Person ("Eve", Gender.Female));

            _initialData = initialDataProvider.Build();
        }

        public IEnumerable<Node> CreateRandomDomain ()
        {
            var result = _dataGenerator.Generate(50, _initialData);
            return new PersonDomainConverter().Convert (result);
        }
    }
}