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
        public void AddParameter<T>(string key, T value) where T : class
        {
            _dictionary.Add(key, value);
        }

        public T GetParameterValue<T>(string key) where T : class
        {
            _dictionary.TryGetValue(key, out object value);
            return value as T;
        }
    }
}
