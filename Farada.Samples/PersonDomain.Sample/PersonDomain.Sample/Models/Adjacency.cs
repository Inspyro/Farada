using System;
using System.Runtime.Serialization;

namespace PersonDomain.Sample.Models
{
    [DataContract]
    public class Adjacency
    {
        [DataMember(Name = "nodeTo")]
        public string NodeTo { get; private set; }

        public Adjacency (Node node)
        {
            NodeTo = node.ID;
        }
    }
}