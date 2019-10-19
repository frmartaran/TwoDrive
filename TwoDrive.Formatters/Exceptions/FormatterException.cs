using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.Formatters.Exceptions
{
    public class FormatterException : Exception
    {
        public FormatterException(string message) : base(message) { }

        public FormatterException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
