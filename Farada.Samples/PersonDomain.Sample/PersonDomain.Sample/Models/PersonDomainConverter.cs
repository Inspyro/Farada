using System;
using System.Collections.Generic;
using Farada.Evolution.RuleBasedDataGeneration;
using PersonDomain.Sample.Models.PersonDomain;

namespace PersonDomain.Sample.Models
{
    public class PersonDomainConverter
    {
        public IEnumerable<Node> Convert (GeneratorResult result)
        {
            var rootNode = new Node ("root");
            var nodes = new List<Node> {rootNode};

            var allPersons = result.GetResult<Person>();
            var personToNode = new Dictionary<Person, Node>();

            var maxOrderY = 0;
            foreach (var person in allPersons)
            {
                var personNode = new Node(string.Format ("{0}{1}Age: {2}", person.Name, "\n", person.Age));
                nodes.Add(personNode);

                personToNode.Add (person, personNode);

                if (person.Father == null || person.Mother == null)
                {
                    rootNode.Adjacencies.Add(new Adjacency(personNode));
                }
                else
                {
                    var fatherNode = personToNode[person.Father];

                    personNode.OrderX = fatherNode.OrderX + 1;
                    personNode.OrderY = fatherNode.OrderY+fatherNode.Adjacencies.Count;

                    maxOrderY = Math.Max (personNode.OrderY, maxOrderY);

                    fatherNode.Adjacencies.Add(new Adjacency(personNode));
                    personToNode[person.Mother].Adjacencies.Add (new Adjacency (personNode));
                }
            }

            rootNode.OrderY = maxOrderY / 2;

            return nodes;
        }
    }
}