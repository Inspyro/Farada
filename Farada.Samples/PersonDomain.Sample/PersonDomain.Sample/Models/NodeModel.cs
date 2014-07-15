using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace PersonDomain.Sample.Models
{
    public class NodeModel
    {
        private static string GetString (XmlObjectSerializer serializer, IEnumerable nodes)
        {
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject (ms, nodes);
                return Encoding.Default.GetString (ms.ToArray());
            }
        }

        public string GenerateGraph (int generations)
        {
            var nodes = new PersonDomainFactory().CreateRandomDomain(generations);

            var serializer = new DataContractJsonSerializer(typeof(IEnumerable<Node>));
            var jsonString = GetString(serializer, nodes);

            return jsonString;
        }
    }
}