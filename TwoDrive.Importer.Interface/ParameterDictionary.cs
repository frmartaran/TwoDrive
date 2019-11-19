using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.Importer.Interface
{
    public class ParameterDictionary : IParameterDictionary
    {
        private IDictionary<string, object> _dictionary;

        public ParameterDictionary()
        {
            _dictionary = new Dictionary<string, object>();
        }
        public void AddParameter<T>(T key, T value)
        {
            throw new NotImplementedException();
        }

        public T GetParameterValue<T>(T key)
        {
            throw new NotImplementedException();
        }
    }
}
