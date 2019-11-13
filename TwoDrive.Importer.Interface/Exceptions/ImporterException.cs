using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.Importer.Interface.Exceptions
{
    public class ImporterException : Exception
    {
        public ImporterException(string message) : base(message) { }

        public ImporterException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
