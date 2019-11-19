using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface.Exceptions;

namespace TwoDrive.Importer.Interface
{
    public class ParameterDictionary : IParameterDictionary
    {
        private IDictionary<string, object> _dictionary;

        public ParameterDictionary()
        {
            _dictionary = new Dictionary<string, object>();
        }
        public void AddParameter<T>(string key, T value) where T : class
        {
            _dictionary.Add(key, value);
        }

        public T GetParameterValue<T>(string key) where T : class
        {
            if(!_dictionary.TryGetValue(key, out object value))
                throw new ParameterException("Value not found for that key");
            return value as T;
        }
    }
}
