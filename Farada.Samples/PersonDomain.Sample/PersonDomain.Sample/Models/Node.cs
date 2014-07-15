using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PersonDomain.Sample.Models
{
    [DataContract]
    public class Node
    {
        private static int s_instanceCount;

         [DataMember(Name = "id")]
        public string ID { get; private set; }

         [DataMember(Name = "name")]
        public string Name { get; private set; }

         [DataMember(Name = "adjacencies")]
        public List<Adjacency> Adjacencies { get; private set; }

         [DataMember(Name = "orderX")]
         public int OrderX { get; set; }

         [DataMember(Name = "orderY")]
         public int OrderY { get; set; }

         public Node(string name, List<Adjacency> adjacencies = null)
        {
            ID = "Node" + s_instanceCount++;
            Name = name;
            Adjacencies = adjacencies ?? new List<Adjacency>();
        }
    }
}