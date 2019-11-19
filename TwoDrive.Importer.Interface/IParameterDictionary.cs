using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.Importer.Interface
{
    public interface IParameterDictionary
    {
        void AddParameter<T>(T key, T value);

        T GetParameterValue<T>(T key);
    }
}
