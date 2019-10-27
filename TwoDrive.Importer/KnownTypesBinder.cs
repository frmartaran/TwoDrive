using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoDrive.Importer
{
    public class KnownTypesBinder : SerializationBinder
    {
        public List<Type> KnownTypes { get; set; }
        public override Type BindToType(string assemblyName, string typeName)
        {
            return KnownTypes.SingleOrDefault(t => t.Name == typeName);
        }
    }
}
