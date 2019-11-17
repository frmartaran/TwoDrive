using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TwoDrive.Importer
{
    public class KnownTypesBinder : ISerializationBinder
    {
        public List<Type> KnownTypes { get; set; }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            throw new NotImplementedException();
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            return KnownTypes.SingleOrDefault(t => t.Name == typeName);
        }
    }
}
