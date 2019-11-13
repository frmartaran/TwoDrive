using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.BusinessLogic.Exceptions
{
    public class ImporterNotFoundException : Exception
    {
        public ImporterNotFoundException(string message) : base(message) { }

        public ImporterNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

    }
}
