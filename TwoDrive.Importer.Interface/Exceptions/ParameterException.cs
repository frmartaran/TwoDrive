using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.Importer.Interface.Exceptions
{
    public class ParameterException : Exception
    {
        public ParameterException(string message) : base(message) { }
    }
}
