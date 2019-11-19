using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.Importer.Interface
{
    public interface IParameterDictionary
    {
        void AddParameter<T>(string key, T value) where T : class;

        T GetParameterValue<T>(string key) where T : class;
    }
}
